namespace BookNest.Models
{
    public class BooksViewModel
    {
        //View yapılarında kullanılan / kullanılacak bazı yapıları bir arada bulunduran bir ViewModel yapısı.
        public Book book { get; set; }

        public List<Book> books { get; set; }

        public Borrower borrower { get; set; }

        public BorrowDetails borrowDetails { get; set; }

        public IFormFile BookImage { set; get; }
    }
}
