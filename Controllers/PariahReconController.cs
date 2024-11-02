using dotnet_cyberpunk_challenge_5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
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

        public PariahReconController( DataContext context )
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

                if( response.IsSuccessStatusCode)
                {
                    List<ArasakaCluster> clusters = response.Content.ReadFromJsonAsync<List<ArasakaCluster>>().Result;

                    return Ok(clusters);
                }
                else
                {
                    return BadRequest($"There was an issue returning the clusters: {response.StatusCode}");
                }                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetArasakaCluster/{id}")]
        public async Task<ActionResult<ArasakaCluster>> GetArasakaCluster( int id )
        {
            try
            {
                HttpResponseMessage response = pariahClient.GetAsync($"api/ArasakaCluster/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    ArasakaCluster updatedCluster = response.Content.ReadFromJsonAsync<ArasakaCluster>().Result;

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
