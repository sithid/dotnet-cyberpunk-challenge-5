using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace dotnet_cyberpunk_challenge_5.Models
{
    public class DataContext : DbContext
    {
        public DbSet<ArasakaCluster> ArasakaClusters { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Process> Processs { get; set; }
        public DbSet<MemoryMapping> MemoryMappings { get; set; }
        public DbSet<AthenaDataEvent> AthenaDataEvents { get; set; }

        public DataContext( DbContextOptions<DataContext> options ) : base( options )
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
