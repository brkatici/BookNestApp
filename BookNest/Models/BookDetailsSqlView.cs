using Microsoft.EntityFrameworkCore;

namespace BookNest.Models
{
    [Keyless]
    public class BookDetailsSqlView
    {
        //SQL view yapısı üzerinden keyless yani primary key olmadan veri alma işlemini gerçekleştiren model
        public int? BookId { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsAvailable { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool? Status { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? isRegistered { get; set; }

    }
}
