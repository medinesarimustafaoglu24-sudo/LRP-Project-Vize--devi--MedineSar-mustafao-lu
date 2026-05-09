


# LRP - Laboratuvar Rehber Portalı 

Bu proje, bir üniversite laboratuvarındaki bilgisayar envanterini yönetmek, cihazları öğrencilere zimmetlemek ve teknik detayları takip etmek amacıyla geliştirilmiş bir **ASP.NET Core** web uygulamasıdır. Proje, Bilgisayar Teknolojisi programı vize ödevi kapsamında hazırlanmıştır.

## Kullanılan Teknolojiler
- **Backend:** .NET 8 (ASP.NET Core Web API)
- **Veritabanı:** SQLite & Entity Framework Core
- **Frontend:** Bootstrap 5 (Özel Toz Pembe Tema), HTML5, CSS3, JavaScript
- **Yönetim:** Fetch API & SessionStorage ile veri yönetimi

## Özellikler
- **Rol Tabanlı Yetkilendirme:** Admin paneli ve Öğrenci portalı için farklı giriş ve yetki seviyeleri.
- **Envanter Yönetimi:** Laboratuvar, bilgisayar ve öğrenci kayıtları üzerinde tam kontrol (CRUD işlemleri).
- **Otomatik Demirbaş Sistemi:** Yeni bir bilgisayar eklendiğinde, bulunduğu laboratuvara göre otomatik `AssetCode` (Örn: LAB1-PC-01) üretimi.
- **Zimmetleme Modülü:** Bilgisayarların öğrenci numarası üzerinden öğrencilere hızlıca atanması.
- **Öğrenci Portalı:** Öğrencilerin kendi üzerlerine zimmetli cihazın işlemci, RAM ve konum bilgilerini görebilmesi.