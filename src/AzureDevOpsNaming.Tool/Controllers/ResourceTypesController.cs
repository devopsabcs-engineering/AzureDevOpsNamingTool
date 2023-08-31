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
    public class ResourceTypesController : ControllerBase
    {
        private readonly IResourceTypeService _resourceTypeService;
        private readonly IAdminLogService _adminLogService;

        public ResourceTypesController(IResourceTypeService resourceTypeService,
            IAdminLogService adminLogService)
        {
            _resourceTypeService = resourceTypeService;
            _adminLogService = adminLogService;
        }

        // GET: api/<ResourceTypesController>
        /// <summary>
        /// This function will return the resource types data. 
        /// </summary>
        /// <returns>json - Current resource types data</returns>
        [HttpGet]
        public async Task<IActionResult> Get(bool admin = false)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                serviceResponse = await _resourceTypeService.GetItems(admin);
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

        // GET api/<ResourceTypesController>/5
        /// <summary>
        /// This function will return the specifed resource type data.
        /// </summary>
        /// <param name="id">int - Resource Type id</param>
        /// <returns>json - Resource Type data</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                serviceResponse = await _resourceTypeService.GetItem(id);
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

        // POST api/<ResourceTypesController>
        /// <summary>
        /// This function will update all resource types data.
        /// </summary>
        /// <param name="items">List - ResourceType (json) - All resource types data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceType> items)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceTypeService.PostConfig(items);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Types updated." });
                    CacheHelper.InvalidateCacheObject("ResourceType");
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

        // POST api/<ResourceTypesController>
        /// <summary>
        /// This function will update all resource types for the specifed component
        /// </summary>
        /// <param name="operation">string - Operation type  (optional-add, optional-remove, exlcude-add, exclude-remove)</param>
        /// <param name="componentid">int - Component ID</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UpdateTypeComponents(string operation, int componentid)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceTypeService.UpdateTypeComponents(operation, componentid);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Types updated." });
                    CacheHelper.InvalidateCacheObject("ResourceType");
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