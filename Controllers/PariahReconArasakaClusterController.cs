using Azure;
using dotnet_cyberpunk_challenge_5.Models;
using dotnet_cyberpunk_challenge_5.Repositories;
using dotnet_cyberpunk_challenge_5.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Collections.Immutable;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace dotnet_cyberpunk_challenge_5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PariahReconArasakaClusterController : ControllerBase
    {
        private readonly IDatabaseRepository _dataRepository;

        private HttpClient pariahClient { get; set; }

        public PariahReconArasakaClusterController(IDatabaseRepository _dataRepo)
        {
            _dataRepository = _dataRepo;

            pariahClient = new HttpClient();
            pariahClient.BaseAddress = new Uri("http://pariah-nexus.blackcypher.io");
            pariahClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        /* Secretes Learned
         * 
         * clusterId: 42
         * clusterName: Cluster-42
         * publicKey: VGhlIEFkbWluIGVuZHBvaW50IGlzIDxVUkw+L2FwaS9OZXRSdW5uZXJBZG1pbmlzdHJhdGlvbg==
         *  base64 Decoded: The Admin endpoint is <URL>/api/NetRunnerAdministration
         * 
         * clusterId: 30
         * clusterName: Cluster-30
         * athenaAccessKey: 7f1b4a78f59a18bf6a216c2173e0de3c
         *  access: authorized
         *  requestUrl: http://pariah-nexus.blackcypher.io/api/ArasakaAthenaDataEvent?athenaKey=7f1b4a78f59a18bf6a216c2173e0de3c
         * 
         */

        [HttpGet("GetAllClusterData")]
        public async Task<ActionResult<IEnumerable<ArasakaCluster>>> GetAllClusterData()
        {
            try
            {
                var clusterListReponse = await pariahClient.GetAsync("api/ArasakaCluster");

                if (clusterListReponse.IsSuccessStatusCode)
                {
                    var arasakaClusters = clusterListReponse.Content.ReadFromJsonAsync<List<ArasakaCluster>>().Result;

                    if (arasakaClusters != null)
                    {
                        var detailedClusters = new List<ArasakaCluster>();

                        foreach (ArasakaCluster lazyCluster in arasakaClusters)
                        {
                            if (lazyCluster != null)
                            {
                                var clusterIdResponse = await pariahClient.GetAsync($"api/ArasakaCluster/{lazyCluster.id}");

                                if (clusterIdResponse.IsSuccessStatusCode)
                                {
                                    var singleDetailedCluster = clusterIdResponse.Content.ReadFromJsonAsync<ArasakaCluster>().Result;

                                    if (singleDetailedCluster != null)
                                    {
                                        await _dataRepository.UpdateData(singleDetailedCluster);
                                        detailedClusters.Add(singleDetailedCluster);
                                    }
                                }
                            }
                        }

                        return Ok(detailedClusters);
                    }
                    else
                        return NotFound($"There were no clusters found.");
                }
                else
                {
                    return BadRequest($"There was an issue returning the clusters: {clusterListReponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong: {ex.Message}");
            }
        }

        [HttpGet("GetSingleClusterById/{id}")]
        public async Task<ActionResult<ArasakaCluster>> GetSingleClusterById(int id)
        {
            try
            {
                var clusterIdResponse = await pariahClient.GetAsync($"api/ArasakaCluster/{id}");

                if (clusterIdResponse.IsSuccessStatusCode)
                {
                    var updatedCluster = await clusterIdResponse.Content.ReadFromJsonAsync<ArasakaCluster>();

                    if (updatedCluster == null)
                        return NotFound();

                    await _dataRepository.UpdateData(updatedCluster);

                    return Ok(updatedCluster);
                }
                else
                {
                    return BadRequest($"The cluster you seek is missing or invalid: {clusterIdResponse.StatusCode} ");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong: {ex.Message}");
            }
        }

        [HttpGet("GetSingleClusterByName/{clusterName}")]
        public async Task<ActionResult<ArasakaCluster>> GetSingleClusterByName(string clusterName )
        {
            try
            {
                var clusterListReponse = await pariahClient.GetAsync("api/ArasakaCluster");

                if (clusterListReponse.IsSuccessStatusCode)
                {
                    var arasakaClusters = clusterListReponse.Content.ReadFromJsonAsync<List<ArasakaCluster>>().Result;

                    if (arasakaClusters != null)
                    {
                        var lazyCluster = arasakaClusters.Find( clusterToFind => string.Equals(clusterToFind.clusterName, clusterName));

                        if (lazyCluster != null)
                        {
                            var clusterIdResponse = pariahClient.GetAsync($"api/ArasakaCluster/{lazyCluster.id}").Result;

                            if (clusterIdResponse.IsSuccessStatusCode)
                            {
                                var detailedCluster = clusterIdResponse.Content.ReadFromJsonAsync<ArasakaCluster>().Result;

                                if (detailedCluster != null)
                                {
                                    await _dataRepository.UpdateData(detailedCluster);
                                    return Ok(detailedCluster);
                                }
                                else
                                    return NotFound("There was no cluster found matching that name.");
                            }
                            else
                                return NotFound("There was no cluster found matching that name.");
                        }
                        else
                            return NotFound("There was no cluster found matching that name.");
                    }
                    else
                        return NotFound($"There were no clusters found.");
                }
                else
                {
                    return BadRequest($"There was an issue returning the clusters: {clusterListReponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong: {ex.Message}");
            }
        }
    }
}
