using System.Text.Json.Serialization;

namespace dotnet_cyberpunk_challenge_5.Models
{
    public class ArasakaDeviceProcess
    {
        public int id { get; set; }
        public string? memory {  get; set; }
        public string? family {  get; set; }
        public string? openFiles {  get; set; }
        public int deviceId {  get; set; }

        [JsonIgnore]
        public ArasakaDevice device { get; set; }
    }
}