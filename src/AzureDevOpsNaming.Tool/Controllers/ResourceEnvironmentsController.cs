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
    public class ResourceEnvironmentsController : ControllerBase
    {
        private readonly IResourceEnvironmentService _resourceEnvironmentService;
        private readonly IAdminLogService _adminLogService;

        public ResourceEnvironmentsController(IResourceEnvironmentService resourceEnvironmentService,
            IAdminLogService adminLogService)
        {
            _resourceEnvironmentService = resourceEnvironmentService;
            _adminLogService = adminLogService;
        }
        // GET: api/<ResourceEnvironmentsController>
        /// <summary>
        /// This function will return the environments data. 
        /// </summary>
        /// <returns>json - Current environments data</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceEnvironmentService.GetItems();
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

        // GET api/<ResourceEnvironmentsController>/5
        /// <summary>
        /// This function will return the specifed environment data.
        /// </summary>
        /// <param name="id">int - Environment id</param>
        /// <returns>json - Environment data</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceEnvironmentService.GetItem(id);
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

        // POST api/<ResourceEnvironmentsController>
        /// <summary>
        /// This function will create/update the specified environment data.
        /// </summary>
        /// <param name="item">ResourceEnvironment (json) - Environment data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResourceEnvironment item)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceEnvironmentService.PostItem(item);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Environment (" + item.Name + ") added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceEnvironment");
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

        // POST api/<ResourceEnvironmentsController>
        /// <summary>
        /// This function will update all environments data.
        /// </summary>
        /// <param name="items">List - ResourceEnvironment (json) - All environments data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceEnvironment> items)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceEnvironmentService.PostConfig(items);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Environments added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceEnvironment");
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

        // DELETE api/<ResourceEnvironmentsController>/5
        /// <summary>
        /// This function will delete the specifed environment data.
        /// </summary>
        /// <param name="id">int - Environment id</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get the item details
                serviceResponse = await _resourceEnvironmentService.GetItem(id);
                if (serviceResponse.Success)
                {
                    ResourceEnvironment item = (ResourceEnvironment)serviceResponse.ResponseObject!;
                    serviceResponse = await _resourceEnvironmentService.DeleteItem(id);
                    if (serviceResponse.Success)
                    {
                        _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Environment (" + item.Name + ") deleted." });
                        CacheHelper.InvalidateCacheObject("ResourceEnvironment");
                        return Ok("Resource Environment (" + item.Name + ") deleted.");
                    }
                    else
                    {
                        return BadRequest(serviceResponse.ResponseObject);
                    }
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
