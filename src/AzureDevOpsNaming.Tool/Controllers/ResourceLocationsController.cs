using AzureNaming.Tool.Attributes;
using AzureNaming.Tool.Helpers;
using AzureNaming.Tool.Models;
using AzureNaming.Tool.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzureNaming.Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    //[TypeFilter(typeof(ApiKeyAttribute))]
    public class ResourceLocationsController : ControllerBase
    {
        private readonly IResourceLocationService _resourceLocationService;
        private readonly IAdminLogService _adminLogService;

        public ResourceLocationsController(IResourceLocationService resourceLocationService,
            IAdminLogService adminLogService)
        {
            _resourceLocationService = resourceLocationService;
            _adminLogService = adminLogService;
        }

        // GET: api/<ResourceLocationsController>
        /// <summary>
        /// This function will return the locations data. 
        /// </summary>
        /// <returns>json - Current locations data</returns>
        [HttpGet]
        public async Task<IActionResult> Get(bool admin = false)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceLocationService.GetItems(admin);
                if (serviceResponse.Success)
                {
                    return Ok(serviceResponse.ResponseObject);
                }
                else
                {
                    return BadRequest(serviceResponse.ResponseObject);
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }

        // GET api/<ResourceLocationsController>/5
        /// <summary>
        /// This function will return the specifed location data.
        /// </summary>
        /// <param name="id">int - Location id</param>
        /// <returns>json - Location data</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceLocationService.GetItem(id);
                if (serviceResponse.Success)
                {
                    return Ok(serviceResponse.ResponseObject);
                }
                else
                {
                    return BadRequest(serviceResponse.ResponseObject);
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }

        // POST api/<ResourceLocationsController>
        /// <summary>
        /// This function will update all locations data.
        /// </summary>
        /// <param name="items">List - ResourceLocation (json) - All locations data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceLocation> items)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceLocationService.PostConfig(items);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Locations added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceLocation");
                    return Ok(serviceResponse.ResponseObject);
                }
                else
                {
                    return BadRequest(serviceResponse.ResponseObject);
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }
    }
}
