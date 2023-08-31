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
    public class ResourceDelimitersController : ControllerBase
    {
        private readonly IResourceDelimiterService _resourceDelimiterService;
        private readonly IAdminLogService _adminLogService;

        public ResourceDelimitersController(IResourceDelimiterService resourceDelimiterService,
            IAdminLogService adminLogService)
        {
            _resourceDelimiterService = resourceDelimiterService;
            _adminLogService = adminLogService;
        }
        // GET api/<ResourceDelimitersController>
        /// <summary>
        /// This function will return the delimiters data.
        /// </summary>
        /// <param name="admin">bool - All/Only-enabled delimiters flag</param>
        /// <returns>json - Current delimiters data</returns>
        [HttpGet]
        public async Task<IActionResult> Get(bool admin = false)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceDelimiterService.GetItems(admin);
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

        // GET api/<ResourceDelimitersController>/5
        /// <summary>
        /// This function will return the specifed resource delimiter data.
        /// </summary>
        /// <param name="id">int - Resource Delimiter id</param>
        /// <returns>json - Resource delimiter data</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                serviceResponse = await _resourceDelimiterService.GetItem(id);
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

        // POST api/<ResourceDelimitersController>
        /// <summary>
        /// This function will create/update the specified delimiter data.
        /// </summary>
        /// <param name="item">ResourceDelimiter (json) - Delimiter data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResourceDelimiter item)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceDelimiterService.PostItem(item);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Delimiter (" + item.Name + ") added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceDelimiter");
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

        // POST api/<resourcedelimitersController>
        /// <summary>
        /// This function will update all delimiters data.
        /// </summary>
        /// <param name="items">List - ResourceDelimiter (json) - All delimiters data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceDelimiter> items)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceDelimiterService.PostConfig(items);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Delimiters added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceDelimiter");
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
