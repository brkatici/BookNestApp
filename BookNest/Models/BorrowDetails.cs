namespace BookNest.Models
{
    public class BorrowDetails
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int BorrowerId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public bool Status { get; set; }    
    }
}
