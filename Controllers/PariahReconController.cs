﻿using dotnet_cyberpunk_challenge_5.Models;
using dotnet_cyberpunk_challenge_5.Repositories;
using dotnet_cyberpunk_challenge_5.Repositories.Contracts;
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
        private readonly IDatabaseRepository _dataRepository;

        public HttpClient pariahClient { get; set; }

        public PariahReconController(IDatabaseRepository _dataRepo)
        {
            _dataRepository = _dataRepo;

            pariahClient = new HttpClient();
            pariahClient.BaseAddress = new Uri("http://pariah-nexus.blackcypher.io");
            pariahClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet("GetArasakaCluster/{id}")]
        public async Task<ActionResult<ArasakaCluster>> GetArasakaCluster(int id)
        {
            try
            {
                var response = await pariahClient.GetAsync($"api/ArasakaCluster/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var updatedCluster = response.Content.ReadFromJsonAsync<ArasakaCluster>().Result;

                    if (updatedCluster == null)
                        return NotFound();

                    await _dataRepository.UpdateData(updatedCluster);

                    return Ok(updatedCluster);
                }
                else
                {
                    return BadRequest($"The cluster you seek is missing or invalid: {response.StatusCode} ");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong: {ex.Message}");
            }
        }

        /* GetArasakaClusters() REMOVED

         After rereading challenge one I have noticed that a generic Get request for all clusters isnt exactly requested.
         Api should accept a GET request for a single cluster by id or name.

         [HttpGet("GetArasakaClusters")]
         public async Task<ActionResult<IEnumerable<ArasakaCluster>>> GetArasakaClusters()
         {
             try
             {
                 var response = pariahClient.GetAsync("api/ArasakaCluster").Result;

                 if (response.IsSuccessStatusCode)
                 {
                     var newClusters = response.Content.ReadFromJsonAsync<List<ArasakaCluster>>().Result;

                     if (newClusters != null)
                     {
                         foreach (var c in newClusters)
                         {
                             await _dataRepository.UpdateData(c);
                         }

                         return Ok(newClusters);
                     }
                     else
                     {
                         return NotFound($"There were no clusters found.");
                     }
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
        */
    }
}
