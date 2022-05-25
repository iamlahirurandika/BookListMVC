using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Models
{
    public class ApplicationDbContext :DbContext
    {
        //Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //We need to add Book to the DB 
        public DbSet<Book> Books { get; set; }
    }
}
