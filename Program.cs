

using LRP_Project_Vize_MedineSarýmustafaoðlu.Data;  
using LRP_Project_Vize_MedineSarýmustafaoðlu.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=lrp.db"));

var app = builder.Build();          

app.UseDefaultFiles();
app.UseStaticFiles();


app.MapPost("/api/auth/login", async (User loginData, AppDbContext db) => {
    var user = await db.Users.FirstOrDefaultAsync(u =>
        u.Username == loginData.Username && u.Password == loginData.Password);

    if (user == null) return Results.Unauthorized();

    var redirectUrl = user.Role == "Admin" ? "index.html" : "student.html";

    return Results.Ok(new
    {
        user.Username,
        user.FullName,
        user.Role,
        Redirect = redirectUrl
    });
});


app.MapGet("/api/labs", async (AppDbContext db) => await db.Labs.ToListAsync());

app.MapPost("/api/labs", async (Lab lab, AppDbContext db) => {
    db.Labs.Add(lab);
    await db.SaveChangesAsync();
    return Results.Created($"/api/labs/{lab.Id}", lab);
});

app.MapDelete("/api/labs/{id}", async (int id, AppDbContext db) => {
    var lab = await db.Labs.FindAsync(id);
    if (lab == null) return Results.NotFound();

    var relatedPcs = await db.Computers.Where(c => c.LabId == id).ToListAsync();
    foreach (var pc in relatedPcs) { pc.LabId = null; }

    db.Labs.Remove(lab);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapPut("/api/labs/{id}", async (int id, Lab updatedLab, AppDbContext db) => {
    var lab = await db.Labs.FindAsync(id);
    if (lab == null) return Results.NotFound();
    lab.Name = updatedLab.Name;
    await db.SaveChangesAsync();
    return Results.Ok(lab);
});

app.MapGet("/api/stats/student-count", async (AppDbContext db) => {
    return await db.Users.CountAsync(u => u.Role == "Student");
});

// --- BÝLGÝSAYAR YÖNETÝMÝ ---
app.MapGet("/api/computers", async (AppDbContext db) => {
    return await db.Computers
        .Include(c => c.Lab)
        .Include(c => c.Student)
        .ToListAsync();
});

app.MapPost("/api/computers", async (Computer pc, AppDbContext db) => {
    int count = await db.Computers.CountAsync(c => c.LabId == pc.LabId) + 1;
    pc.AssetCode = $"LAB{pc.LabId}-PC-{count:D2}";
    db.Computers.Add(pc);
    await db.SaveChangesAsync();
    return Results.Ok(pc);
});

app.MapDelete("/api/computers/{id}", async (int id, AppDbContext db) => {
    var pc = await db.Computers.FindAsync(id);
    if (pc == null) return Results.NotFound();
    db.Computers.Remove(pc);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// --- ÖÐRENCÝ YÖNETÝMÝ ---
app.MapGet("/api/users/students", async (AppDbContext db) => {
    return await db.Users
        .Where(u => u.Role == "Student")
        .Include(u => u.Computers)
        .ToListAsync();
});

app.MapPost("/api/users/students", async (User student, AppDbContext db) => {
    var existing = await db.Users.AnyAsync(u => u.Username == student.Username);
    if (existing) return Results.BadRequest("Bu öðrenci numarasý zaten kayýtlý!");

    student.Role = "Student";
    if (string.IsNullOrEmpty(student.Password)) student.Password = "12345";

    db.Users.Add(student);
    await db.SaveChangesAsync();
    return Results.Ok(student);
});

app.MapDelete("/api/users/students/{id}", async (int id, AppDbContext db) => {
    var user = await db.Users.FindAsync(id);
    if (user == null) return Results.NotFound();

    var assignedPcs = await db.Computers.Where(c => c.StudentId == id).ToListAsync();
    foreach (var pc in assignedPcs) { pc.StudentId = null; }

    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.Ok();
});


app.MapPost("/api/assign-direct", async (int pcId, int studentId, AppDbContext db) => {
    var pc = await db.Computers.FindAsync(pcId);
    var student = await db.Users.FindAsync(studentId);

    if (pc == null || student == null) return Results.NotFound("Bilgisayar veya öðrenci bulunamadý.");

    pc.StudentId = student.Id; 
    await db.SaveChangesAsync();

    return Results.Ok(new { message = "Zimmetleme Baþarýlý" });
});


app.MapPost("/api/assign", async (int pcId, string studentNo, string fullName, AppDbContext db) => {
    var user = await db.Users.FirstOrDefaultAsync(u => u.Username == studentNo);
    if (user == null)
    {
        user = new User { Username = studentNo, FullName = fullName, Password = "12345", Role = "Student" };
        db.Users.Add(user);
        await db.SaveChangesAsync();
    }
    var pc = await db.Computers.FindAsync(pcId);
    if (pc != null) { pc.StudentId = user.Id; await db.SaveChangesAsync(); }
    return Results.Ok();
});


app.MapGet("/api/student/my-device/{username}", async (string username, AppDbContext db) => {
    var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
    if (user == null) return Results.NotFound();

    var pc = await db.Computers
        .Include(c => c.Lab)
        .FirstOrDefaultAsync(c => c.StudentId == user.Id);

    return pc != null ? Results.Ok(pc) : Results.NotFound("Size atanmýþ bilgisayar bulunamadý.");
});


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    if (!context.Users.Any())
    {
        context.Users.Add(new User { Username = "admin", Password = "123", FullName = "Admin Kullanýcý", Role = "Admin" });
        context.Users.Add(new User { Username = "241902039", Password = "123", FullName = "Medine Öðrenci", Role = "Student" });
    }

    if (!context.Labs.Any())
    {
        context.Labs.Add(new Lab { Name = "Yazýlým Laboratuvarý 1" });
    }
    context.SaveChanges();
}

app.Run();