using BookNest.Models;
using Microsoft.EntityFrameworkCore;

namespace BookNest.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Borrower> Borrowers { get; set; }

        public DbSet<BorrowDetails> BorrowDetails { get; set; }

        public DbSet<BookDetailsSqlView> AllBookDetails {  get; set; } // SQl view yapısındaki veriler ile ORM bağlantısı yap.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<BookDetailsSqlView>(
                    eb =>
                    {
                        eb.HasNoKey(); // Keyless yani primary key olmayan bir tablo sorgulayacağımızı belirttik.
                        eb.ToView("View_BookDetails"); // Sorgulanacak olan SQL View
                    });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("LibManagementDb");
            }
        }

        
    }
}
