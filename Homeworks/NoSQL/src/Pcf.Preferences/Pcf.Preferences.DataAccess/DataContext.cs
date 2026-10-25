using Microsoft.EntityFrameworkCore;
using Pcf.Preferences.Core.Domain;

namespace Pcf.Preferences.DataAccess
{
    public class DataContext
        : DbContext
    {
        public DbSet<Preference> Preferences { get; set; }

        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
