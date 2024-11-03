using dotnet_cyberpunk_challenge_5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace dotnet_cyberpunk_challenge_5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PariahReconController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public HttpClient pariahClient { get; set; }

        public PariahReconController(DataContext context)
        {
            _dataContext = context;

            pariahClient = new HttpClient();
            pariahClient.BaseAddress = new Uri("http://pariah-nexus.blackcypher.io");
            pariahClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet("GetArasakaClusters")]
        public async Task<ActionResult<IEnumerable<ArasakaCluster>>> GetArasakaClusters()
        {
            try
            {
                HttpResponseMessage response = pariahClient.GetAsync("api/ArasakaCluster").Result;

                if (response.IsSuccessStatusCode)
                {
                    var newClusters = response.Content.ReadFromJsonAsync<List<ArasakaCluster>>().Result;

                    if (newClusters != null)
                    {
                        foreach (ArasakaCluster c in newClusters)
                        {
                            var toAdd = new ArasakaCluster()
                            {
                                clusterName = c.clusterName,
                                nodeCount = c.nodeCount,
                                cpuCores = c.cpuCores,
                                memoryGb = c.memoryGb,
                                storageTb = c.storageTb,
                                creationDate = c.creationDate,
                                environment = c.environment,
                                kubernetesVersion = c.kubernetesVersion,
                                region = c.region,
                            };

                            if (c.devices != null)
                            {
                                toAdd.devices = new List<Device>();

                                foreach (Device d in c.devices)
                                {
                                    var deviceToAdd = new Device()
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

                                        foreach (Process p in d.processes)
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
                                            _dataContext.SaveChanges();
                                        }
                                    }

                                    if (d.memoryMappings != null)
                                    {
                                        deviceToAdd.memoryMappings = new List<MemoryMapping>();

                                        foreach (MemoryMapping m in d.memoryMappings)
                                        {
                                            var mapping = new MemoryMapping()
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
                                            _dataContext.SaveChanges();
                                        }
                                    }

                                    if (d.dataEvents != null)
                                    {
                                        deviceToAdd.dataEvents = new List<AthenaDataEvent>();

                                        foreach (AthenaDataEvent e in d.dataEvents)
                                        {
                                            var dataEvent = new AthenaDataEvent()
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
                                            _dataContext.SaveChanges();
                                        }
                                    }

                                    toAdd.devices.Add(deviceToAdd);
                                    _dataContext.Devices.Add(deviceToAdd);
                                    _dataContext.SaveChanges();
                                }
                                _dataContext.ArasakaClusters.Add(toAdd);
                                _dataContext.SaveChanges();
                            }
                        }
                    }
                    return Ok(newClusters);
                }
                else
                {
                    return BadRequest($"There was an issue returning the clusters: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong: {ex.Message}");
            }
        }

        [HttpGet("GetArasakaCluster/{id}")]
        public async Task<ActionResult<ArasakaCluster>> GetArasakaCluster(int id)
        {
            try
            {
                HttpResponseMessage response = pariahClient.GetAsync($"api/ArasakaCluster/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    ArasakaCluster updatedCluster = response.Content.ReadFromJsonAsync<ArasakaCluster>().Result;

                    if (updatedCluster == null)
                        return NotFound();

                    var toAdd = new ArasakaCluster()
                    {
                        clusterName = updatedCluster.clusterName,
                        nodeCount = updatedCluster.nodeCount,
                        cpuCores = updatedCluster.cpuCores,
                        memoryGb = updatedCluster.memoryGb,
                        storageTb = updatedCluster.storageTb,
                        creationDate = updatedCluster.creationDate,
                        environment = updatedCluster.environment,
                        kubernetesVersion = updatedCluster.kubernetesVersion,
                        region = updatedCluster.region,
                    };

                    if (updatedCluster.devices != null)
                    {
                        toAdd.devices = new List<Device>();

                        foreach (Device d in updatedCluster.devices)
                        {
                            var deviceToAdd = new Device()
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
                                    _dataContext.SaveChanges();
                                }
                            }

                            if (d.memoryMappings != null)
                            {
                                deviceToAdd.memoryMappings = new List<MemoryMapping>();

                                foreach (MemoryMapping m in deviceToAdd.memoryMappings)
                                {
                                    var mapping = new MemoryMapping()
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
                                    _dataContext.SaveChanges();
                                }
                            }

                            if (d.dataEvents != null)
                            {
                                deviceToAdd.dataEvents = new List<AthenaDataEvent>();

                                foreach (AthenaDataEvent e in d.dataEvents)
                                {
                                    var dataEvent = new AthenaDataEvent()
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
                                    _dataContext.SaveChanges();
                                }
                            }
                            toAdd.devices.Add(deviceToAdd);
                            _dataContext.Devices.Add(deviceToAdd);
                            _dataContext.SaveChanges();
                        }

                        _dataContext.ArasakaClusters.Add(toAdd);
                        _dataContext.SaveChanges();
                    }

                    return Ok(updatedCluster);
                }
                else
                {
                    return BadRequest($"The cluster you seek is missing or invalid: {response.StatusCode} ");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool ClusterExists(int id)
        {
            return _dataContext.ArasakaClusters.Any(c => c.id == id);
        }
    }
}
