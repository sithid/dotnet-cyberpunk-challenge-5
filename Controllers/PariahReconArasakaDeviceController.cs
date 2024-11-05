using dotnet_cyberpunk_challenge_5.Models;
using dotnet_cyberpunk_challenge_5.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_cyberpunk_challenge_5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PariahReconArasakaDeviceController : ControllerBase
    {
        private readonly IDatabaseRepository _dataRepository;
        private HttpClient pariahClient { get; set; }

        public PariahReconArasakaDeviceController(IDatabaseRepository dataRepository)
        {
            _dataRepository = dataRepository;

            pariahClient = new HttpClient();
            pariahClient.BaseAddress = new Uri("http://pariah-nexus.blackcypher.io");
            pariahClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet("GetArasakaDevices")]
        public async Task<ActionResult<List<ArasakaDevice>>> GetArasakaDevices()
        {
            try
            {
                var arasakaDevicesResponse = await pariahClient.GetAsync("api/ArasakaDevice");

                if (arasakaDevicesResponse.IsSuccessStatusCode)
                {
                    var devices = await arasakaDevicesResponse.Content.ReadFromJsonAsync<List<ArasakaDevice>>();

                    if (devices == null)
                        return NotFound("No devices found");

                    return Ok(devices);
                }
                else
                {
                    return BadRequest("Bad Request");
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Bad request!");
            }
        }


        [HttpGet("GetSingleDeviceById/{id}")]
        public async Task<ActionResult<ArasakaDevice>> GetArasakaDevice(int id)
        {
            try
            {
                var deviceIdResponse = await pariahClient.GetAsync($"api/ArasakaDevice/{id}");

                if (deviceIdResponse.IsSuccessStatusCode)
                {
                    var device = await deviceIdResponse.Content.ReadFromJsonAsync<ArasakaDevice>();

                    if (device == null)
                        return NotFound("There was no device with that id found.");

                    return Ok(device);
                }
                else
                    return BadRequest(deviceIdResponse.StatusCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
