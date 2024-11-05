using dotnet_cyberpunk_challenge_5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace dotnet_cyberpunk_challenge_5.Controllers
{
    public class DataContext : DbContext
    {
        public DbSet<ArasakaCluster> ArasakaClusters { get; set; }
        public DbSet<ArasakaDevice> Devices { get; set; }
        public DbSet<ArasakaDeviceProcess> Processs { get; set; }
        public DbSet<ArasakaDeviceMemoryMapping> MemoryMappings { get; set; }
        public DbSet<ArasakaAthenaDataEvent> AthenaDataEvents { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
