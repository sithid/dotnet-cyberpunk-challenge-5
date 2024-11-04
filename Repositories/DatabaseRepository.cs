using dotnet_cyberpunk_challenge_5.Models;
using dotnet_cyberpunk_challenge_5.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace dotnet_cyberpunk_challenge_5.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly DataContext _dataContext;

        public DatabaseRepository( DataContext context )
        {
            _dataContext = context;
        }

        public async Task<List<ArasakaCluster>> GetArasakaClustersAsync()
        {
            var clusters = await _dataContext.ArasakaClusters.ToListAsync();

            return clusters;
        }

        public async Task<ArasakaCluster> GetArasakaClusterAsync( int id )
        {
            var existingCluster = await _dataContext.ArasakaClusters
                .Include(c => c.devices)
                .ThenInclude(d => d.processes)
                .Include(c => c.devices)
                .ThenInclude(d => d.memoryMappings)
                .Include(c => c.devices)
                .ThenInclude(d => d.dataEvents)
                .FirstOrDefaultAsync(c => c.id == id);

            return existingCluster;
        }
    }
}
