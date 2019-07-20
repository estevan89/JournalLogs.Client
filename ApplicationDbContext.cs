using Microsoft.EntityFrameworkCore;

namespace client
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<JournalLog> JournalLogs { get; set; }
    }
}