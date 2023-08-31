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
    public class ResourceNamingRequestsController : ControllerBase
    {
        private readonly IResourceNamingRequestService _resourceNamingRequestService;
        private readonly IAdminLogService _adminLogService;
        private readonly IResourceTypeService _resourceTypeService;

        public ResourceNamingRequestsController(IResourceNamingRequestService resourceNamingRequestService,
            IAdminLogService adminLogService,
            IResourceTypeService resourceTypeService)
        {
            _resourceNamingRequestService = resourceNamingRequestService;
            _adminLogService = adminLogService;
            _resourceTypeService = resourceTypeService;
        }

        // POST api/<ResourceNamingRequestsController>
        /// <summary>
        /// This function will generate a resoure type name for specifed component values. This function requires full definition for all components. It is recommended to use the RequestName API function for name generation.   
        /// </summary>
        /// <param name="request">ResourceNameRequestWithComponents (json) - Resource Name Request data</param>
        /// <returns>string - Name generation response</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> RequestNameWithComponents([FromBody] ResourceNameRequestWithComponents request)
        {
            try
            {
                ResourceNameResponse resourceNameRequestResponse = await _resourceNamingRequestService.RequestNameWithComponents(request);
                if (resourceNameRequestResponse.Success)
                {
                    return Ok(resourceNameRequestResponse);
                }
                else
                {
                    return BadRequest(resourceNameRequestResponse);
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ResourceNamingRequestsController>
        /// <summary>
        /// This function will generate a resoure type name for specifed component values, using a simple data format.  
        /// </summary>
        /// <param name="request">ResourceNameRequest (json) - Resource Name Request data</param>
        /// <returns>string - Name generation response</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> RequestName([FromBody] ResourceNameRequest request)
        {
            try
            {
                request.CreatedBy = "API";
                ResourceNameResponse resourceNameRequestResponse = await _resourceNamingRequestService.RequestName(request);
                if (resourceNameRequestResponse.Success)
                {
                    return Ok(resourceNameRequestResponse);
                }
                else
                {
                    return BadRequest(resourceNameRequestResponse);
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ResourceNamingRequestsController>
        /// <summary>
        /// This function will validate the name for the specified resource type.  
        /// </summary>
        /// <param name="validateNameRequest">ValidateNameRequest (json) - Validate Name Request data</param>
        /// <returns>ValidateNameResponse - Name validation response</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ValidateName([FromBody] ValidateNameRequest validateNameRequest)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get the current delimiter
                serviceResponse = await _resourceTypeService.ValidateResourceTypeName(validateNameRequest);
                if (serviceResponse.Success)
                {
                    if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                    {
                        ValidateNameResponse validateNameResponse = (ValidateNameResponse)serviceResponse.ResponseObject!;
                        return Ok(validateNameResponse);
                    }
                    else
                    {
                        return BadRequest("There was a problem validating the name.");
                    }
                }
                else
                {
                    return BadRequest("There was a problem validating the name.");
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex.Message);
            }
        }
    }
}
