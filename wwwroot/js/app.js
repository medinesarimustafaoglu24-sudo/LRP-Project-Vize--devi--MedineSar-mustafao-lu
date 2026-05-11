



async function updateStudentTable() {
    const studs = await (await fetch('/api/users/students')).json();
    const tableBody = document.getElementById('student-table-body');

    tableBody.innerHTML = studs.map(s => {
        const assignedPcs = s.computers && s.computers.length > 0
            ? s.computers.map(pc => `<span class="badge bg-custom-pink me-1">${pc.assetCode}</span>`).join('')
            : '<span class="text-muted small">Yok</span>';

        return `
            <tr>
                <td><b>${s.username}</b></td>
                <td>${s.fullName}</td>
                <td>${assignedPcs}</td>
                <td><button onclick="deleteStudent(${s.id})" class="btn btn-sm btn-danger">🗑️ Sil</button></td>
            </tr>`;
    }).join('');
}