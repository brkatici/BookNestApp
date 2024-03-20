namespace BookNest.Models
{
    public class Borrower
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        
        public bool isRegistered { get; set; }
    }

}
