
# BookNest

Bu uygulama, bir kütüphane yönetim sistemi olan BookNest'i oluşturmayı amaçlamaktadır. Kullanıcılar, kitapları ödünç verebilir, iade alabilir ve kütüphane envanterini görebilirler. Ayrıca kitap bilgisi ve görseli ile yeni kitap ekleyebilirler.


## Kullanılan Teknolojiler
**Visual Studio**

**Back End:** .NetCore 7.0 , C# , MVC, EF Core , Migrations , ViewModels

**Front End:** Tiny Dashboard (free template) , DataTables , JQuery ,Ajax , Javascript , HTML , CSS , SweetAlert2

**MSSQL:** SQL, SQL Tables , SQL Views


  
## Özellikler

- Kitap bilgisi ve görseli ile kitap ekleme
- Kullanıcı / okuyucuya kitap verme
- Kitabı iade alma
- Tüm kitap envanterini listeleme

  
## Optimizasyon

- Sıralama, sayfalama , listeleme özelliklerinin kullanıcı tarafını basitleştirmek adına JQuery Datatables eklentisi entegre edildi.
- https://datatables.net/

- Kullanıcı tarafı hatalarını karşılamak , kullanıcıyı bilgilendirmek amacıyla SweetAlert2.js entegre edildi.

- https://sweetalert2.github.io/
  
# Paket ve Versiyon Gereksinimleri

- Microsoft.EntityFrameworkCore v7.0.17
- Microsoft.EntityFrameworkCore.SqlServer v7.0.17
- Microsoft.EntityFrameworkCore.Tools v7.0.17
- Microsoft.VisualStudio.Web.CodeGeneration.Design v7.0.12

## Nasıl Çalışır ?
- Yeni Kitap Ekle butonuna tıklayın.

- Kitap bilgilerini girin ve ardından bir dosya seçerek kitap görselini ekleyin.

- "Yeni Kitap Ekle" butonuna tekrar tıklayın ve kitabın başarıyla kaydedildiğine dair bir uyarı aldıktan sonra devam edin.

- "Kitap Listesi" sekmesine dönün ve eklediğiniz kitabı listede göreceksiniz.

- Her kitabın yanında bulunan "Ödünç Ver" butonunu kullanarak kitabı başka bir kullanıcıya ödünç verebilirsiniz.

- Bu adımda kullanıcı veya okuyucu bilgilerini girmeniz gerekecektir.

- Ödünç verilen kitap geri getirildiğinde, "Bilgi" butonuna tıklayarak kitabın durumunu kontrol edebilirsiniz. Ardından "Kitap Geri Getirildi" butonuna tıklayarak kitabı tekrar listeleyebilirsiniz, böylece diğer kullanıcılar da kullanabilir.

Bu adımları takip ederek BookNest'i kolayca kullanabilirsiniz.
## Pull Request ve Kurulum
- Projeyi klonlayın veya indirin.
- Proje dizinine gidin.
- appsettings.json dosyasını düzenleyerek veritabanı bağlantı dizesini belirtin.
-Your_ServerName alanına oluşturulacak olan veritabanı adını girin:
- "ConnectionStrings": {
  "LibManagementDb": "Server=Your_ServerName;Database=LibManagementDb;Trusted_Connection=true; TrustServerCertificate=True;MultipleActiveResultSets=true"
}

- Tercihen yukarıdaki menüden View => Other Windows => Package Manager Console tıklayın , açılan console`da " update-database " komutu ile migration dosyalarını veritabanına işleyin.

- Uygulamayı başlatmak için terminalde veya komut istemcisinde dotnet run komutunu çalıştırın.

- Ya da yeşil ok işaretine tıklayın

- Tarayıcınızda https://localhost adresine giderek uygulamayı görüntüleyin.