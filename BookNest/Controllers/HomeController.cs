using BookNest.Data;
using BookNest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookNest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }


        //Kitapların listelendiği view yapısı
        //Liste MSSQL View üzerinden sorgulanır
        //SQL view üzerinde 3 tablodaki detaylar (books,borrowers,borrowdetails) birleştirilmiştir.

        public IActionResult Index()
        {
            try
            {
                var bookDetails = _appDbContext.AllBookDetails.OrderBy(x => x.Title).ToList();
                return View(bookDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching book details.");
                return StatusCode(500, new { success = false, message = "An error occurred while fetching book details." });
            }
        }

        // Kitap verme işlemini yapacak Bootstrap-Modal yapısına, ihtiyaç duyulan verileri BooksViewModel üzerinden doldurup gönderir.
        //Ön yüzde Javascript ile yapılan istek doğrultusunda doldurulan PartialView, Modal-Body içerisinde Load edilir.
        public async Task<IActionResult> LoadLendingModal(int id)
        {
            try
            {
                var viewModel = new BooksViewModel();
                viewModel.book = await _appDbContext.Books.FirstOrDefaultAsync(x => x.Id == id);

                if (viewModel.book == null)
                    return NotFound();

                if (!viewModel.book.IsAvailable)
                {
                    viewModel.borrowDetails = await _appDbContext.BorrowDetails.FirstOrDefaultAsync(x => x.BookId == id && x.Status);
                    viewModel.borrower = await _appDbContext.Borrowers.FirstOrDefaultAsync(y => y.Id == viewModel.borrowDetails.BorrowerId);
                }

                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading lending modal.");
                return StatusCode(500, new { success = false, message = "An error occurred while loading lending modal." });
            }
        }

        //Kitap verme işlemi.
        //Borrower yani kitabı alan kişi veritabanı üzerinde oluşturulur.
        //Kitap bilgisi veritabanı üzerinden sorgulanır
        //BorrowBookFromLibrary fonksiyonu ile kitap ıd ve ödünç alan kişinin Id bilgileri ortak bir tabloya insert edilir
        [HttpPost]
        public async Task<IActionResult> LendTheBook(BooksViewModel model)
        {
            try
            {
                var borrower = await GetOrCreateBorrower(model);
                if (borrower == null)
                    return BadRequest(new { success = false, message = "Borrower bilgileri alınamadı veya oluşturulamadı." });

                var book = await _appDbContext.Books.FirstOrDefaultAsync(x => x.Id == model.book.Id);
                if (book == null || !book.IsAvailable)
                    return BadRequest(new { success = false, message = "Kitap mevcut değil veya zaten ödünç alınmış." });

                await BorrowBookFromLibrary(book, borrower, model);

                return Ok(new { success = true, message = "Kitap başarıyla ödünç verildi." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while lending the book.");
                return StatusCode(500, new { success = false, message = "An error occurred while lending the book." });
            }
        }

        private async Task<Borrower> GetOrCreateBorrower(BooksViewModel model)
        {
            try
            {
                if (model.borrower.Id == 0)
                {
                    _appDbContext.Borrowers.Add(model.borrower);
                    await _appDbContext.SaveChangesAsync();
                }

                return await _appDbContext.Borrowers.FirstOrDefaultAsync(b => b.Id == model.borrower.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting or creating borrower.");
                return null;
            }
        }

        private async Task BorrowBookFromLibrary(Book book, Borrower borrower, BooksViewModel model)
        {
            try
            {
                book.IsAvailable = false;
                var borrowDetails = new BorrowDetails
                {
                    BookId = book.Id,
                    BorrowerId = borrower.Id,
                    BorrowDate = DateTime.Now,
                    ReturnDate = model.borrowDetails.ReturnDate,
                    Status = true
                };

                _appDbContext.BorrowDetails.Add(borrowDetails);
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while borrowing book from library.");
            }
        }

        //Kitabı geri alma işlemi. Kullanıcı / okuyucu kitabı iade ettiğinde,
        //Kitapla ilgili ödünç verme işlemi veritabanındaki BorrowDetails üzerinden sorgulanır
        //İşlem veritabanından silinir , kitap durumu Available yani true olarak işaretlenir.
        //Alternatif olarak işlem tablosundan tamamen silmek yerine status kolonu true false şeklinde değiştirilerek, bu tablonun gerektiğinde bir 
        //işlem kaydı tutan log tablosu görevi görmesi de sağlanabilir.
        //Kitap yeniden ödünç alma işlemine hazırdır.
        [HttpPost]
        public async Task<IActionResult> ReturnBook(BooksViewModel model)
        {
            try
            {
                var borrowDetail = await _appDbContext.BorrowDetails.FindAsync(model.borrowDetails.Id);

                if (borrowDetail == null)
                    return NotFound();

                _appDbContext.BorrowDetails.Remove(borrowDetail);
                var book = await _appDbContext.Books.FindAsync(borrowDetail.BookId);
                book.IsAvailable = true;
                await _appDbContext.SaveChangesAsync();

                return Ok(new { success = true, message = "Kitap okuyucudan geri alındı." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while returning book.");
                return StatusCode(500, new { success = false, message = "An error occurred while returning book." });
            }
        }
    }
}
