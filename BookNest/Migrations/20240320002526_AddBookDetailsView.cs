using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookNest.Migrations
{
    /// <inheritdoc />
    public partial class AddBookDetailsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
            CREATE VIEW View_BookDetails AS
            SELECT        dbo.Books.Title, dbo.Books.Author, dbo.Books.ImageUrl, dbo.BorrowDetails.BorrowDate, dbo.BorrowDetails.ReturnDate, dbo.Borrowers.Name, dbo.Borrowers.Surname, dbo.Borrowers.Email, dbo.Borrowers.PhoneNumber, 
                         dbo.BorrowDetails.Status, dbo.Books.IsAvailable, dbo.BorrowDetails.BorrowerId, dbo.Borrowers.isRegistered, dbo.Books.Id AS BookId
FROM            dbo.Books LEFT OUTER JOIN
                         dbo.BorrowDetails ON dbo.Books.Id = dbo.BorrowDetails.BookId LEFT OUTER JOIN
                         dbo.Borrowers ON dbo.BorrowDetails.BorrowerId = dbo.Borrowers.Id");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"DROP VIEW View_BookDetails");
        }
    }
}
