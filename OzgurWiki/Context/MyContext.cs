using Microsoft.EntityFrameworkCore;
using OzgurWiki.Models;

namespace OzgurWiki.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
            : base(options) { }
        public DbSet<WikiPage> WikiPages { get; set; }
    }
}
