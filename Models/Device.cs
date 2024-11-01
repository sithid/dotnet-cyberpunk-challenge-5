namespace dotnet_cyberpunk_challenge_5.Models
{
    public class Device
    {
        public int id { get; set; }
        public string name {  get; set; }
        public string architecture {  get; set; }
        public string processorType {  get; set; }
        public string region {  get; set; }
        public string athenaAccessKey {  get; set; }
        public int clusterId {  get; set; }        
        public List<Process>? processess {  get; set; }
        public List<MemoryMapping>? memoryMappings { get; set; }
        public List<AthenaDataEvent>? dataEvents { get; set; }
    }
}