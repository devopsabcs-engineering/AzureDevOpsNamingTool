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
    public class ResourceComponentsController : ControllerBase
    {
        private readonly IResourceComponentService _resourceComponentService;
        private readonly IAdminLogService _adminLogService;

        public ResourceComponentsController(
            IResourceComponentService resourceComponentService,
            IAdminLogService adminLogService)
        {
            _resourceComponentService = resourceComponentService;
            _adminLogService = adminLogService;
        }
        // GET: api/<resourcecomponentsController>
        /// <summary>
        /// This function will return the components data.
        /// </summary>
        /// <param name="admin">bool - All/Only-enabled components flag</param>
        /// <returns>json - Current components data</returns>
        [HttpGet]
        public async Task<IActionResult> Get(bool admin = false)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceComponentService.GetItems(admin);
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

        // GET api/<resourcecomponentsController>/5
        /// <summary>
        /// This function will return the specifed resource component data.
        /// </summary>
        /// <param name="id">int - Resource Component id</param>
        /// <returns>json - Resource component data</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                serviceResponse = await _resourceComponentService.GetItem(id);
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

        // POST api/<ResourceComponentsController>
        /// <summary>
        /// This function will create/update the specified component data.
        /// </summary>
        /// <param name="item">ResourceComponent (json) - Component data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResourceComponent item)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceComponentService.PostItem(item);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Component (" + item.Name + ") added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceComponent");
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

        // POST api/<ResourceComponentsController>
        /// <summary>
        /// This function will update all components data.
        /// </summary>
        /// <param name="items">List - ResourceComponent (json) - All components configuration data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceComponent> items)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceComponentService.PostConfig(items);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Components added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceComponent");
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
