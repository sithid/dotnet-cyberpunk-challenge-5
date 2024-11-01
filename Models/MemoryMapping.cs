namespace dotnet_cyberpunk_challenge_5.Models
{
    public class MemoryMapping
    {
        public string memoryType {  get; set; }
        public float memorySizeGb { get; set; }
        public int memorySpeedMhz { get; set; }
        public string memoryTechnology {  get; set; }
        public int memoryLatency {  get; set; }
        public float memoryVoltage {  get; set; }
        public string memoryFormFactor {  get; set; }
        public int memoryEccSupport {  get; set; }
        public int memoryHeatSpreader {  get; set; }
        public int memoryWarrantyYears {  get; set; }
        public int deviceId {  get; set; }
    }
}
