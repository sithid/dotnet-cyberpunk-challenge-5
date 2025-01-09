using dotnet_cyberpunk_challenge_5.Controllers;
using dotnet_cyberpunk_challenge_5.Models;
using dotnet_cyberpunk_challenge_5.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel;
using System.Diagnostics.Metrics;

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

        public async Task<ArasakaCluster> GetArasakaClusterAsync(int id)
        {
            var existingCluster = await _dataContext.ArasakaClusters
                .Include(cluster => cluster.devices)
                .ThenInclude(device => device.processes)
                .Include(cluster => cluster.devices)
                .ThenInclude(device => device.memoryMappings)
                .Include(cluster => cluster.devices)
                .ThenInclude(device => device.dataEvents)
                .FirstOrDefaultAsync(cluster => cluster.clusterName == $"Cluster-{id}");                
                
            return existingCluster;
        }

        public async Task UpdateData( ArasakaCluster data )
        {
            var oldCluster = GetArasakaClusterAsync(data.id).Result;
            var newCluster = BuildArasakaCluster(data);

            if (oldCluster != null)
            {
                Console.WriteLine("oldCluster != null");

                if (oldCluster.devices != null)
                {
                    foreach (ArasakaDevice oldDevice in oldCluster.devices)
                    {
                        if (oldDevice.processes != null)
                        {
                            _dataContext.RemoveRange(oldDevice.processes);
                            _dataContext.SaveChanges();
                        }

                        if (oldDevice.memoryMappings != null)
                        {
                            _dataContext.RemoveRange(oldDevice.memoryMappings);
                            _dataContext.SaveChanges();
                        }

                        if (oldDevice.dataEvents != null)
                        {
                            _dataContext.RemoveRange(oldDevice.dataEvents);
                            _dataContext.SaveChanges();
                        }
                    }

                    _dataContext.RemoveRange(oldCluster.devices);
                    _dataContext.SaveChanges();
                }

                _dataContext.Remove(oldCluster);
                _dataContext.SaveChanges();
            }

            _dataContext.Add(newCluster);
            _dataContext.SaveChanges();
        }

        public ArasakaCluster BuildArasakaCluster( ArasakaCluster clusterToCopy )
        {
            var clusterNoId = new ArasakaCluster
            {
                clusterName = clusterToCopy.clusterName,
                nodeCount = clusterToCopy.nodeCount,
                cpuCores = clusterToCopy.cpuCores,
                memoryGb = clusterToCopy.memoryGb,
                storageTb = clusterToCopy.storageTb,
                creationDate = clusterToCopy.creationDate,
                environment = clusterToCopy.environment,
                kubernetesVersion = clusterToCopy.kubernetesVersion,
                region = clusterToCopy.region,
            };

            if (clusterToCopy.devices != null)
            {
                clusterNoId.devices = new List<ArasakaDevice>();

                foreach (ArasakaDevice oldDevice in clusterToCopy.devices)
                {
                    ArasakaDevice deviceNoId = BuildDevice(oldDevice);
                    clusterNoId.devices.Add(deviceNoId);
                }
            }

            return clusterNoId;
        }

        public ArasakaDevice BuildDevice( ArasakaDevice deviceToCopy )
        {
            var deviceNoId = new ArasakaDevice
            {
                name = deviceToCopy.name,
                publicKey = deviceToCopy.publicKey,
                architecture = deviceToCopy.architecture,
                processorType = deviceToCopy.processorType,
                region = deviceToCopy.region,
                athenaAccessKey = deviceToCopy.athenaAccessKey,
                clusterId = deviceToCopy.clusterId
            };

            if (deviceToCopy.processes != null)
            {
                deviceNoId.processes = new List<ArasakaDeviceProcess>();

                foreach (ArasakaDeviceProcess p in deviceToCopy.processes)
                {
                    ArasakaDeviceProcess processNoId = BuildProcess(p);
                    deviceNoId.processes.Add(processNoId);
                }
            }

            if (deviceToCopy.memoryMappings != null)
            {
                deviceNoId.memoryMappings = new List<ArasakaDeviceMemoryMapping>();

                foreach (ArasakaDeviceMemoryMapping mm in deviceToCopy.memoryMappings)
                {
                    ArasakaDeviceMemoryMapping mappingNoId = BuildMemoryMapping(mm);
                    deviceNoId.memoryMappings.Add(mappingNoId);
                }
            }

            if(deviceToCopy.dataEvents != null)
            {
                deviceNoId.dataEvents = new List<ArasakaAthenaDataEvent>();

                foreach( ArasakaAthenaDataEvent athenaDataEvent in deviceToCopy.dataEvents )
                {
                    ArasakaAthenaDataEvent eventNoId = BuildAthenaDataEvent(athenaDataEvent);
                    deviceNoId.dataEvents.Add(eventNoId);
                }
            }

            return deviceNoId;
        }

        public ArasakaDeviceProcess BuildProcess( ArasakaDeviceProcess process )
        {
            return new ArasakaDeviceProcess()
            {
                memory = process.memory,
                family = process.family,
                openFiles = process.openFiles,
                deviceId = process.deviceId,
            };
        }

        public ArasakaDeviceMemoryMapping BuildMemoryMapping( ArasakaDeviceMemoryMapping memoryMapping )
        {
            return new ArasakaDeviceMemoryMapping
            {
                memoryType = memoryMapping.memoryType,
                memorySizeGb = memoryMapping.memorySizeGb,
                memorySpeedMhz = memoryMapping.memorySpeedMhz,
                memoryTechnology = memoryMapping.memoryTechnology,
                memoryLatency = memoryMapping.memoryLatency,
                memoryVoltage = memoryMapping.memoryVoltage,
                memoryFormFactor = memoryMapping.memoryFormFactor,
                memoryEccSupport = memoryMapping.memoryEccSupport,
                memoryHeatSpreader = memoryMapping.memoryHeatSpreader,
                memoryWarrantyYears = memoryMapping.memoryWarrantyYears,
                deviceId = memoryMapping.deviceId
            };
        }

        public ArasakaAthenaDataEvent BuildAthenaDataEvent( ArasakaAthenaDataEvent athenaDataEvent )
        {
            return new ArasakaAthenaDataEvent
            {
                userId = athenaDataEvent.userId,
                ipAddress = athenaDataEvent.ipAddress,
                macAddress = athenaDataEvent.macAddress,
                eventTimestamp = athenaDataEvent.eventTimestamp,
                eventType = athenaDataEvent.eventType,
                source = athenaDataEvent.source,
                severity = athenaDataEvent.severity,
                location = athenaDataEvent.location,
                userAgent = athenaDataEvent.userAgent,
                deviceBrand = athenaDataEvent.deviceBrand,
                deviceModel = athenaDataEvent.deviceModel,
                osVersion = athenaDataEvent.osVersion,
                appName = athenaDataEvent.appName,
                appVersion = athenaDataEvent.appVersion,
                errorCode = athenaDataEvent.errorCode,
                errorMessage = athenaDataEvent.errorMessage,
                responseTime = athenaDataEvent.responseTime,
                success = athenaDataEvent.success,
                deviceId = athenaDataEvent.deviceId
            };
        }
    }
}
