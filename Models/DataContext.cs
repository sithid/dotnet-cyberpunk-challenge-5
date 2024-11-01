using Microsoft.EntityFrameworkCore;

namespace dotnet_cyberpunk_challenge_5.Models
{
    public class DataContext : DbContext
    {
        public DataContext( DbContextOptions<DataContext> options ) : base( options )
        {

        }

        public DbSet<ArasakaCluster> ArasakaClusters { get; set; }
    }
}
