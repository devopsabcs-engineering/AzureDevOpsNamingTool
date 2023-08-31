using AzureNaming.Tool.Attributes;
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
    public class ImportExportController : ControllerBase
    {
        private readonly IImportExportService _importExportService;
        private readonly IAdminLogService _adminLogService;

        public ImportExportController(IImportExportService importExportService, IAdminLogService adminLogService)
        {
            _importExportService = importExportService;
            _adminLogService = adminLogService;
        }

        // GET: api/<ImportExportController>
        /// <summary>
        /// This function will export the current configuration data (all components) as a single JSON file. 
        /// </summary>
        /// <returns>json - JSON configuration file</returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ExportConfiguration(bool includeAdmin = false)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _importExportService.ExportConfig(includeAdmin);
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

        // POST api/<ImportExportController>
        /// <summary>
        /// This function will import the provided configuration data (all components). This will overwrite the existing configuration data. 
        /// </summary>
        /// <param name="configdata">ConfigurationData (json) - Tool configuration File</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ImportConfiguration([FromBody] ConfigurationData configdata)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _importExportService.PostConfig(configdata);
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
    }
}
