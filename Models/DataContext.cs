using Microsoft.EntityFrameworkCore;

namespace dotnet_cyberpunk_challenge_5.Models
{
    public class DataContext : DbContext
    {
        public DataContext( DbContextOptions<DataContext> options ) : base( options )
        {

        }

        public DbSet<ArasakaCluster> ArasakaClusters { get; set; }
        public DbSet<AthenaDataEvent> AthenaDataEvents { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Process> Processs { get; set; }
        public DbSet<MemoryMapping> MemoryMappings { get; set; }
    }
}
