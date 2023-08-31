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
    public class ResourceFunctionsController : ControllerBase
    {
        private readonly IResourceFunctionService _resourceFunctionService;
        private readonly IAdminLogService _adminLogService;

        public ResourceFunctionsController(IResourceFunctionService resourceFunctionService,
            IAdminLogService adminLogService)
        {
            _resourceFunctionService = resourceFunctionService;
            _adminLogService = adminLogService;
        }

        // GET: api/<ResourceFunctionsController>
        /// <summary>
        /// This function will return the functions data. 
        /// </summary>
        /// <returns>json - Current functions data</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                serviceResponse = await _resourceFunctionService.GetItems();
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

        // GET api/<ResourceFunctionsController>/5
        /// <summary>
        /// This function will return the specifed function data.
        /// </summary>
        /// <param name="id">int - Function id</param>
        /// <returns>json - Function data</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceFunctionService.GetItem(id);
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

        // POST api/<ResourceFunctionsController>
        /// <summary>
        /// This function will create/update the specified function data.
        /// </summary>
        /// <param name="item">ResourceFunction (json) - Function data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResourceFunction item)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceFunctionService.PostItem(item);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Function (" + item.Name + ") added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceFunction");
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

        // POST api/<ResourceFunctionsController>
        /// <summary>
        /// This function will update all functions data.
        /// </summary>
        /// <param name="items">List - ResourceFunction (json) - All functions data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceFunction> items)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceFunctionService.PostConfig(items);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Functions added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceFunction");
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

        // DELETE api/<ResourceFunctionsController>/5
        /// <summary>
        /// This function will delete the specifed function data.
        /// </summary>
        /// <param name="id">int - Function id</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get the item details
                serviceResponse = await _resourceFunctionService.GetItem(id);
                if (serviceResponse.Success)
                {
                    ResourceFunction item = (ResourceFunction)serviceResponse.ResponseObject!;
                    serviceResponse = await _resourceFunctionService.DeleteItem(id);
                    if (serviceResponse.Success)
                    {
                        _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Function (" + item.Name + ") deleted." });
                        CacheHelper.InvalidateCacheObject("ResourceFunction");
                        return Ok("Resource Function (" + item.Name + ") deleted.");
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