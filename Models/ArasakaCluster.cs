namespace dotnet_cyberpunk_challenge_5.Models
{
    public class ArasakaCluster
    {
        public string clusterName {  get; set; }
        public int nodeCount {  get; set; }
        public int cpuCores {  get; set; }
        public int memoryGb { get; set; }
        public int storageTb { get; set; }
        public string creationDate {  get; set; }
        public string environment {  get; set; }
        public string kubernetesVersion {  get; set; }
        public string region {  get; set; }
        public List<Device> devices {  get; set; }
    }
}
