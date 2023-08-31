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
    public class CustomComponentsController : ControllerBase
    {
        private readonly ICustomComponentService _customComponentService;
        private readonly IAdminLogService _adminLogService;
        private readonly IResourceComponentService _resourceComponentService;

        public CustomComponentsController(
            ICustomComponentService customComponentService,
            IAdminLogService adminLogService,
            IResourceComponentService resourceComponentService)
        {
            _customComponentService = customComponentService;
            _adminLogService = adminLogService;
            _resourceComponentService = resourceComponentService;
        }

        // GET: api/<CustomComponentsController>
        /// <summary>
        /// This function will return the custom components data. 
        /// </summary>
        /// <returns>json - Current custom components data</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                serviceResponse = await _customComponentService.GetItems();
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

        // GET api/<CustomComponentsController>/sample
        /// <summary>
        /// This function will return the custom components data for the specifc parent component type.
        /// </summary>
        /// <param name="parenttype">string - Parent Component Type Name</param>
        /// <returns>json - Current custom components data</returns>
        [Route("[action]/{parenttype}")]
        [HttpGet]
        public async Task<IActionResult> GetByParentType(string parenttype)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                serviceResponse = await _customComponentService.GetItemsByParentType(GeneralHelper.NormalizeName(parenttype, true));
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

        // GET api/<CustomComponentsController>/5
        /// <summary>
        /// This function will return the specifed custom component data.
        /// </summary>
        /// <param name="id">int - Custom Component id</param>
        /// <returns>json - Custom component data</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                serviceResponse = await _customComponentService.GetItem(id);
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

        // POST api/<CustomComponentsController>
        /// <summary>
        /// This function will create/update the specified custom component data.
        /// </summary>
        /// <param name="item">CustomComponent (json) - Custom component data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomComponent item)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _customComponentService.PostItem(item);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Custom Component (" + item.Name + ") updated." });
                    CacheHelper.InvalidateCacheObject("CustomComponent");
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

        // POST api/<CustomComponentsController>
        /// <summary>
        /// This function will update all custom components data.
        /// </summary>
        /// <param name="items">List-CustomComponent (json) - All custom components data. (Legacy functionality).</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<CustomComponent> items)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _customComponentService.PostConfig(items);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Custom Components updated." });
                    CacheHelper.InvalidateCacheObject("CustomComponent");
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

        // POST api/<CustomComponentsController>
        /// <summary>
        /// This function will update all custom components data.
        /// </summary>
        /// <param name="config">CustomComponmentConfig (json) - Full custom components data with parent component data.</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfigWithParentData([FromBody] CustomComponmentConfig config)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                List<ResourceComponent> currentresourcecomponents = new();
                List<CustomComponent> newcustomcomponents = new();
                // Get the current resource components
                serviceResponse = await _resourceComponentService.GetItems(true);
                if (serviceResponse.Success)
                {
                    if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                    {
                        currentresourcecomponents = serviceResponse.ResponseObject!;

                        // Loop through the posted components
                        if (GeneralHelper.IsNotNull(config.ParentComponents))
                        {
                            foreach (ResourceComponent thisparentcomponent in config.ParentComponents)
                            {
                                // Check if the posted component exists in the current components
                                if (!currentresourcecomponents.Exists(x => x.Name == thisparentcomponent.Name))
                                {
                                    // Add the custom component
                                    ResourceComponent newcustomcomponent = new()
                                    {
                                        Name = thisparentcomponent.Name,
                                        DisplayName = thisparentcomponent.Name,
                                        IsCustom = true
                                    };
                                    serviceResponse = await _resourceComponentService.PostItem(newcustomcomponent);

                                    if (serviceResponse.Success)
                                    {
                                        // Add the new custom component to the list
                                        currentresourcecomponents.Add(newcustomcomponent);
                                    }
                                    else
                                    {
                                        return BadRequest(serviceResponse.ResponseObject);
                                    }
                                }
                            }
                        }
                    }

                    if (GeneralHelper.IsNotNull(config.CustomComponents))
                    {
                        if (config.CustomComponents.Count > 0)
                        {
                            // Loop through custom components to make sure the parent exists
                            foreach (CustomComponent thiscustomcomponent in config.CustomComponents)
                            {
                                if (currentresourcecomponents.Where(x => GeneralHelper.NormalizeName(x.Name, true) == thiscustomcomponent.ParentComponent).Any())
                                {
                                    newcustomcomponents.Add(thiscustomcomponent);
                                }
                            }

                            // Update the custom component options
                            serviceResponse = await _customComponentService.PostConfig(newcustomcomponents);
                            if (!serviceResponse.Success)
                            {
                                return BadRequest(serviceResponse.ResponseObject);
                            }
                        }
                    }
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Custom Components updated." });
                    CacheHelper.InvalidateCacheObject("CustomComponent");
                    return Ok("Custom Component configuration updated!");
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

        // DELETE api/<CustomComponentsController>/5
        /// <summary>
        /// This function will delete the specifed custom component data.
        /// </summary>
        /// <param name="id">int - Custom component id</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get the item details
                serviceResponse = await _customComponentService.GetItem(id);
                if (serviceResponse.Success)
                {
                    CustomComponent item = (CustomComponent)serviceResponse.ResponseObject!;
                    serviceResponse = await _customComponentService.DeleteItem(id);
                    if (serviceResponse.Success)
                    {
                        _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Custom Component (" + item.Name + ") deleted." });
                        CacheHelper.InvalidateCacheObject("GeneratedName");
                        return Ok("Custom Component (" + item.Name + ") deleted.");
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