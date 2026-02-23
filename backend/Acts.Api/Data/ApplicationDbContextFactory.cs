using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Acts.Api.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ActsDbContext>
    {
        public ActsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ActsDbContext>();
            optionsBuilder.UseSqlite("Data Source=app.db");

            return new ActsDbContext(optionsBuilder.Options);
        }
    }
}