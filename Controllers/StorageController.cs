using dotnet_cyberpunk_challenge_5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_cyberpunk_challenge_5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public HttpClient pariahClient { get; set; }

        public StorageController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpGet("GetArasakaClusters")]
        public async Task<ActionResult<IEnumerable<ArasakaCluster>>> GetArasakaClusters()
        {
            return await _dataContext.ArasakaClusters.ToListAsync();
        }

        [HttpGet("GetArasakaCluster/{id}")]
        public async Task<ActionResult<ArasakaCluster>> GetArasakaCluster(int id)
        {
            var cluster = await _dataContext.ArasakaClusters
                .Include(c => c.devices)
                .ThenInclude(d => d.processes)
                .Include( c => c.devices)
                .ThenInclude(d => d.memoryMappings)
                .Include(c => c.devices)
                .ThenInclude(d => d.dataEvents)
                .FirstOrDefaultAsync(c => c.id == id);

            if (cluster == null)
            {
                return NotFound();
            }

            return Ok(cluster);
        }
    }
}
