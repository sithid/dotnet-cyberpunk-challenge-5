using dotnet_cyberpunk_challenge_5.Models;
using dotnet_cyberpunk_challenge_5.Repositories;
using dotnet_cyberpunk_challenge_5.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_cyberpunk_challenge_5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IDatabaseRepository _dataRepository;

        public HttpClient pariahClient { get; set; }

        public StorageController(IDatabaseRepository _dataRepo)
        {
            _dataRepository = _dataRepo;
        }

        [HttpGet("GetArasakaClusters")]
        public async Task<ActionResult<IEnumerable<ArasakaCluster>>> GetArasakaClusters()
        {
            List<ArasakaCluster> clusters = await _dataRepository.GetArasakaClustersAsync();

            if (clusters.Any())
                return Ok(clusters);
            else
                return NotFound("No clusters found.");
        }

        [HttpGet("GetArasakaCluster/{id}")]
        public async Task<ActionResult<ArasakaCluster>> GetArasakaCluster(int id)
        {
            var cluster = await _dataRepository.GetArasakaClusterAsync(id);

            if (cluster == null)
                return NotFound($"Cluster {id} was not found.");

            return Ok(cluster);
        }
    }
}
