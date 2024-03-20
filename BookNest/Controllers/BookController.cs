using BookNest.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using BookNest.Data;

namespace BookNest.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _environment;

        public BookController(IWebHostEnvironment environment, AppDbContext appDbContext)
        {
            _environment = environment;
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }


        // Yeni kitap bilgisi ekleyen fonksiyon
        [HttpPost]
        public async Task<IActionResult> AddNewBook(BooksViewModel model, IFormFile bookImage)
        {
            if (bookImage == null || bookImage.Length == 0)
            {
                return BadRequest(new { success = false, message = "Kitap fotoğrafı yüklenmedi." });
            }

            if (!IsImageFile(bookImage))
            {
                return BadRequest(new { success = false, message = "Lütfen geçerli bir resim dosyası yükleyin." });
            }

            if (bookImage.Length > 5 * 1024 * 1024) // 5 MB'den büyük dosyaları engelle
            {
                return BadRequest(new { success = false, message = "Dosya boyutu 5 MB'tan büyük olamaz." });
            }

            var uploadFolder = Path.Combine(_environment.WebRootPath, "BookImages"); // wwwroot altında BookImages adlı klasöre yerleştir , yoksa oluştur.

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(bookImage.FileName); // Kitap görsellerinin unique Id ile işaretlenmesi
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await bookImage.CopyToAsync(fileStream);
            }
            //Görseli kaydet

            model.book.ImageUrl = "BookImages/" + uniqueFileName; // Url oluştur ve book nesnesindeki ilgili alana kaydet.
            model.book.IsAvailable = true;

            try
            {
                _appDbContext.Books.Add(model.book);
                await _appDbContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Kitap bilgileri başarıyla kaydedildi." });
            }
            catch (Exception ex)
            {
                // Hata durumunda dosyayı sil
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                return BadRequest(new { success = false, message = "Kitap bilgileri kaydedilirken bir hata oluştu." });
            }
        }

        private bool IsImageFile(IFormFile file)
        {
            if (file.ContentType.ToLower() != "image/jpg" &&
                file.ContentType.ToLower() != "image/jpeg" &&
                file.ContentType.ToLower() != "image/pjpeg" &&
                file.ContentType.ToLower() != "image/gif" &&
                file.ContentType.ToLower() != "image/x-png" &&
                file.ContentType.ToLower() != "image/png")
            {
                return false;
            }
            return true;
        }
    }
}

