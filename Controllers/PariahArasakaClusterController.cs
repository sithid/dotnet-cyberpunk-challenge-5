using dotnet_cyberpunk_challenge_5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;

namespace dotnet_cyberpunk_challenge_5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PariahArasakaClusterController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public HttpClient pariahClient { get; set; }

        public PariahArasakaClusterController( DataContext context )
        {
            _dataContext = context;

            pariahClient = new HttpClient();
            pariahClient.BaseAddress = new Uri("http://pariah-nexus.blackcypher.io");
            pariahClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public IActionResult GetArasakaClusters()
        {
            try
            {
                HttpResponseMessage response = pariahClient.GetAsync("api/ArasakaCluster").Result;          

                if( response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;

                    List<ArasakaCluster> clusters = JsonConvert.DeserializeObject<List<ArasakaCluster>>(json);

                    return Ok(clusters);
                }
                else
                {
                    return BadRequest("Something went wrong in the request.  What'd you fuck up?");
                }                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetArasakaCluster( int id )
        {
            try
            {
                HttpResponseMessage response = pariahClient.GetAsync($"api/ArasakaCluster/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result; // if you print this string, you will see the json collection for processes with data

                    ArasakaCluster cluster = JsonConvert.DeserializeObject<ArasakaCluster>(json); // when deserialized, processes is null

                    return Ok(cluster);
                }
                else
                {
                    return BadRequest("Something went wrong in the request.  What'd you fuck up?");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
