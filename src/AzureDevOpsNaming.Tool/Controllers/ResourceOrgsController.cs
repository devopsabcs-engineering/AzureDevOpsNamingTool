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
    public class ResourceOrgsController : ControllerBase
    {
        private readonly IResourceOrgService _resourceOrgService;
        private readonly IAdminLogService _adminLogService;

        public ResourceOrgsController(IResourceOrgService resourceOrgService,
            IAdminLogService adminLogService)
        {
            _resourceOrgService = resourceOrgService;
            _adminLogService = adminLogService;
        }

        // GET: api/<ResourceOrgsController>
        /// <summary>
        /// This function will return the orgs data. 
        /// </summary>
        /// <returns>json - Current orgs data</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                serviceResponse = await _resourceOrgService.GetItems();
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

        // GET api/<ResourceOrgsController>/5
        /// <summary>
        /// This function will return the specifed org data.
        /// </summary>
        /// <param name="id">int - Org id</param>
        /// <returns>json - Org data</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                serviceResponse = await _resourceOrgService.GetItem(id);
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

        // POST api/<ResourceOrgsController>
        /// <summary>
        /// This function will create/update the specified org data.
        /// </summary>
        /// <param name="item">ResourceOrg (json) - Org data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResourceOrg item)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceOrgService.PostItem(item);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Org (" + item.Name + ") added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceOrg");
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

        // POST api/<ResourceOrgsController>
        /// <summary>
        /// This function will update all orgs data.
        /// </summary>
        /// <param name="items">List - ResourceOrg (json) - All orgs data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceOrg> items)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceOrgService.PostConfig(items);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Orgs added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceOrg");
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

        // DELETE api/<ResourceOrgsController>/5
        /// <summary>
        /// This function will delete the specifed org data.
        /// </summary>
        /// <param name="id">int - Org id</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get the item details
                serviceResponse = await _resourceOrgService.GetItem(id);
                if (serviceResponse.Success)
                {
                    ResourceOrg item = (ResourceOrg)serviceResponse.ResponseObject!;
                    serviceResponse = await _resourceOrgService.DeleteItem(id);
                    if (serviceResponse.Success)
                    {
                        _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Org (" + item.Name + ") deleted." });
                        CacheHelper.InvalidateCacheObject("ResourceOrg");
                        return Ok("Resource Org (" + item.Name + ") deleted.");
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