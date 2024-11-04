using dotnet_cyberpunk_challenge_5.Models;

namespace dotnet_cyberpunk_challenge_5.Repositories.Contracts
{
    public interface IDatabaseRepository
    {
        Task<List<ArasakaCluster>> GetArasakaClustersAsync();
        Task<ArasakaCluster> GetArasakaClusterAsync( int id );
        Task UpdateData( ArasakaCluster cluster );
        ArasakaCluster BuildArasakaCluster(ArasakaCluster cluster);
        Device BuildDevice(Device device);
        Process BuildProcess(Process process);
        MemoryMapping BuildMemoryMapping(MemoryMapping memoryMapping);
        AthenaDataEvent BuildAthenaDataEvent(AthenaDataEvent athenaDataEvent);
    }
}
