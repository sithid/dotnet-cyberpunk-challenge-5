using dotnet_cyberpunk_challenge_5.Models;
using dotnet_cyberpunk_challenge_5.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

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
            var existingCluster = await _dataContext.ArasakaClusters.FindAsync(id);

            return existingCluster;
        }

        public async Task<bool> UpdateData( ArasakaCluster cluster )
        {
            try
            {
                var newCluster = new ArasakaCluster
                {
                    clusterName = cluster.clusterName,
                    nodeCount = cluster.nodeCount,
                    cpuCores = cluster.cpuCores,
                    memoryGb = cluster.memoryGb,
                    storageTb = cluster.storageTb,
                    creationDate = cluster.creationDate,
                    environment = cluster.environment,
                    kubernetesVersion = cluster.kubernetesVersion,
                    region = cluster.region,
                };

                if (cluster.devices != null)
                {
                    newCluster.devices = new List<Device>();

                    foreach (Device d in cluster.devices)
                    {
                        var deviceToAdd = new Device
                        {
                            name = d.name,
                            publicKey = d.publicKey,
                            architecture = d.architecture,
                            processorType = d.processorType,
                            region = d.region,
                            athenaAccessKey = d.athenaAccessKey,
                            clusterId = d.clusterId,
                        };

                        if (d.processes != null)
                        {
                            deviceToAdd.processes = new List<Process>();

                            foreach (Process p in deviceToAdd.processes)
                            {
                                var proc = new Process()
                                {
                                    memory = p.memory,
                                    family = p.family,
                                    openFiles = p.openFiles,
                                    deviceId = p.deviceId,
                                };

                                deviceToAdd.processes.Add(proc);
                                _dataContext.Processs.Add(proc);
                            }
                        }

                        if (d.memoryMappings != null)
                        {
                            deviceToAdd.memoryMappings = new List<MemoryMapping>();

                            foreach (MemoryMapping m in deviceToAdd.memoryMappings)
                            {
                                var mapping = new MemoryMapping
                                {
                                    memoryType = m.memoryType,
                                    memorySizeGb = m.memorySizeGb,
                                    memorySpeedMhz = m.memorySpeedMhz,
                                    memoryTechnology = m.memoryTechnology,
                                    memoryLatency = m.memoryLatency,
                                    memoryVoltage = m.memoryVoltage,
                                    memoryFormFactor = m.memoryFormFactor,
                                    memoryEccSupport = m.memoryEccSupport,
                                    memoryHeatSpreader = m.memoryHeatSpreader,
                                    memoryWarrantyYears = m.memoryWarrantyYears,
                                    deviceId = m.deviceId,
                                };

                                deviceToAdd.memoryMappings.Add(mapping);
                                _dataContext.MemoryMappings.Add(mapping);
                            }
                        }

                        if (d.dataEvents != null)
                        {
                            deviceToAdd.dataEvents = new List<AthenaDataEvent>();

                            foreach (AthenaDataEvent e in d.dataEvents)
                            {
                                var dataEvent = new AthenaDataEvent
                                {
                                    userId = e.userId,
                                    ipAddress = e.ipAddress,
                                    macAddress = e.macAddress,
                                    eventTimestamp = e.eventTimestamp,
                                    eventType = e.eventType,
                                    source = e.source,
                                    severity = e.severity,
                                    location = e.location,
                                    userAgent = e.userAgent,
                                    deviceBrand = e.deviceBrand,
                                    deviceModel = e.deviceModel,
                                    osVersion = e.osVersion,
                                    appName = e.appName,
                                    appVersion = e.appVersion,
                                    errorCode = e.errorCode,
                                    errorMessage = e.errorMessage,
                                    responseTime = e.responseTime,
                                    success = e.success,
                                    deviceId = e.deviceId,
                                };

                                deviceToAdd.dataEvents.Add(dataEvent);
                                _dataContext.AthenaDataEvents.Add(dataEvent);
                            }
                        }

                        newCluster.devices.Add(deviceToAdd);
                        _dataContext.Devices.Add(deviceToAdd);
                    }

                    var existingCluster = await GetArasakaClusterAsync(cluster.id);

                    if (existingCluster != null)
                    {
                        _dataContext.Entry(existingCluster).CurrentValues.SetValues(newCluster);
                        await _dataContext.SaveChangesAsync();
                        return true;
                    }

                    _dataContext.ArasakaClusters.Add(newCluster);
                    await _dataContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex}");
                return false;
            }

            return true;
        }
    }
}
