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

        public async Task<ArasakaCluster> GetArasakaClusterAsync( int id )
        {
            var existingCluster = await _dataContext.ArasakaClusters
                .Include(cluster => cluster.devices)
                .ThenInclude(device => device.processes)
                .Include(cluster => cluster.devices)
                .ThenInclude(device => device.memoryMappings)
                .Include(cluster => cluster.devices)
                .ThenInclude(device => device.dataEvents)
                .FirstOrDefaultAsync(cluster => cluster.id == id);

            return existingCluster;
        }

        public async Task UpdateData( ArasakaCluster data )
        {
            var oldCluster = await GetArasakaClusterAsync(data.id);

            var newCluster = BuildArasakaCluster(data);

            if (oldCluster != null)
            {
                if (oldCluster.devices != null)
                {
                    foreach (Device oldDevice in oldCluster.devices)
                    {
                        _dataContext.AthenaDataEvents.RemoveRange(oldDevice.dataEvents);
                        _dataContext.MemoryMappings.RemoveRange(oldDevice.memoryMappings);
                        _dataContext.Processs.RemoveRange(oldDevice.processes);
                    }

                    _dataContext.Devices.RemoveRange(oldCluster.devices);
                }

                _dataContext.ArasakaClusters.Remove(oldCluster);
            }

            _dataContext.ArasakaClusters.Add(newCluster);
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
                clusterNoId.devices = new List<Device>();

                foreach (Device oldDevice in clusterToCopy.devices)
                {
                    Device deviceNoId = BuildDevice(oldDevice);
                    clusterNoId.devices.Add(deviceNoId);
                }
            }

            return clusterNoId;
        }

        public Device BuildDevice( Device deviceToCopy )
        {
            var deviceNoId = new Device
            {
                name = deviceToCopy.name,
                publicKey = deviceToCopy.publicKey,
                architecture = deviceToCopy.architecture,
                processorType = deviceToCopy.processorType,
                region = deviceToCopy.region,
                athenaAccessKey = deviceToCopy.athenaAccessKey,
            };

            if (deviceToCopy.processes != null)
            {
                deviceToCopy.processes = new List<Process>();

                foreach (Process p in deviceToCopy.processes)
                {
                    Process processNoId = BuildProcess(p);
                    deviceToCopy.processes.Add(processNoId);
                }
            }

            if (deviceToCopy.memoryMappings != null)
            {
                deviceToCopy.memoryMappings = new List<MemoryMapping>();

                foreach (MemoryMapping mm in deviceToCopy.memoryMappings)
                {
                    MemoryMapping mappingNoId = BuildMemoryMapping(mm);
                    deviceToCopy.memoryMappings.Add(mappingNoId);
                }
            }

            if(deviceToCopy.dataEvents != null)
            {
                deviceToCopy.dataEvents = new List<AthenaDataEvent>();

                foreach( AthenaDataEvent athenaDataEvent in deviceToCopy.dataEvents )
                {
                    AthenaDataEvent eventNoId = BuildAthenaDataEvent(athenaDataEvent);
                    deviceToCopy.dataEvents.Add(eventNoId);
                }
            }

            return deviceToCopy;
        }

        public  Process BuildProcess( Process process )
        {
            return new Process()
            {
                memory = process.memory,
                family = process.family,
                openFiles = process.openFiles,
            };
        }

        public MemoryMapping BuildMemoryMapping( MemoryMapping memoryMapping )
        {
            return new MemoryMapping
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
            };
        }

        public AthenaDataEvent BuildAthenaDataEvent( AthenaDataEvent athenaDataEvent )
        {
            return new AthenaDataEvent
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
            };
        }
    }
}
