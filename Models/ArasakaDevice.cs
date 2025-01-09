using System.Text.Json.Serialization;

namespace dotnet_cyberpunk_challenge_5.Models
{
    public class ArasakaDevice
    {
        public int id { get; set; }
        public string? name {  get; set; }
        public string? publicKey { get; set; }
        public string? architecture {  get; set; }
        public string? processorType {  get; set; }
        public string? region {  get; set; }
        public string? athenaAccessKey {  get; set; }
        public int clusterId {  get; set; }

        [JsonIgnore]
        public ArasakaCluster cluster { get; set; }

        public List<ArasakaDeviceProcess>? processes {  get; set; }
        public List<ArasakaDeviceMemoryMapping>? memoryMappings { get; set; }
        public List<ArasakaAthenaDataEvent>? dataEvents { get; set; }
    }
}