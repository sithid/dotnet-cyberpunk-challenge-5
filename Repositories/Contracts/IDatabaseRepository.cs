using dotnet_cyberpunk_challenge_5.Models;

namespace dotnet_cyberpunk_challenge_5.Repositories.Contracts
{
    public interface IDatabaseRepository
    {
        Task<List<ArasakaCluster>> GetArasakaClustersAsync();
        Task<ArasakaCluster> GetArasakaClusterAsync( int id );
        Task UpdateData( ArasakaCluster cluster );
        ArasakaCluster BuildArasakaCluster(ArasakaCluster cluster);
        ArasakaDevice BuildDevice(ArasakaDevice device);
        ArasakaDeviceProcess BuildProcess(ArasakaDeviceProcess process);
        ArasakaDeviceMemoryMapping BuildMemoryMapping(ArasakaDeviceMemoryMapping memoryMapping);
        ArasakaAthenaDataEvent BuildAthenaDataEvent(ArasakaAthenaDataEvent athenaDataEvent);
    }
}
