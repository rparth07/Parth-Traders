using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace Parth_Traders.Data;
public class DesignContextFactory : IDesignTimeDbContextFactory<ParthTradersContext>
{
    public ParthTradersContext CreateDbContext(string[] args) => new(new DbContextOptionsBuilder<ParthTradersContext>()
        .UseNpgsql("Host=localhost;Database=parth_design;Username=design;Password=design").Options);
}
