using AzureNaming.Tool.Attributes;
using AzureNaming.Tool.Helpers;
using AzureNaming.Tool.Models;
using AzureNaming.Tool.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzureNaming.Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    //[TypeFilter(typeof(ApiKeyAttribute))]
    public class AdminController : ControllerBase
    {
        private readonly SiteConfiguration _config = ConfigurationHelper.GetConfigurationData();
        private readonly IGeneratedNamesService _generatedNamesService;
        private readonly IAdminService _adminService;
        private readonly IAdminLogService _adminLogService;

        public AdminController(
            IGeneratedNamesService generatedNamesService,
            IAdminService adminService,
            IAdminLogService adminLogService)
        {
            _generatedNamesService = generatedNamesService;
            _adminService = adminService;
            _adminLogService = adminLogService;
        }

        // POST api/<AdminController>
        /// <summary>
        /// This function will update the Global Admin Password. 
        /// </summary>
        /// <param name="password">string - New Global Admin Password</param>
        /// <param name="adminpassword">string - Current Global Admin Password</param>
        /// <returns>string - Successful update</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UpdatePassword([BindRequired][FromHeader(Name = "AdminPassword")] string adminpassword, [FromBody] string password)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                if (GeneralHelper.IsNotNull(adminpassword))
                {
                    if (adminpassword == GeneralHelper.DecryptString(_config.AdminPassword!, _config.SALTKey!))
                    {
                        serviceResponse = await _adminService.UpdatePassword(password);
                        return (serviceResponse.Success ? Ok("SUCCESS") : Ok("FAILURE - There was a problem updating the password."));
                    }
                    else
                    {
                        return Ok("FAILURE - Incorrect Global Admin Password.");
                    }

                }
                else
                {
                    return Ok("FAILURE - You must provide the Global Admin Password.");
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }

        // POST api/<AdminController>
        /// <summary>
        /// This function will update the API Key. 
        /// </summary>
        /// <param name="apikey">string - New API Key</param>
        /// <param name="adminpassword">string - Current Global Admin Password</param>
        /// <returns>dttring - Successful update</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UpdateAPIKey([BindRequired][FromHeader(Name = "AdminPassword")] string adminpassword, [FromBody] string apikey)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                if (GeneralHelper.IsNotNull(adminpassword))
                {
                    if (adminpassword == GeneralHelper.DecryptString(_config.AdminPassword!, _config.SALTKey!))
                    {
                        serviceResponse = await _adminService.UpdateAPIKey(apikey);
                        return (serviceResponse.Success ? Ok("SUCCESS") : Ok("FAILURE - There was a problem updating the API Key."));
                    }
                    else
                    {
                        return Ok("FAILURE - Incorrect Global Admin Password.");
                    }

                }
                else
                {
                    return Ok("FAILURE - You must provide the Global Admin Password.");
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }

        // POST api/<AdminController>
        /// <summary>
        /// This function will generate a new API Key. 
        /// </summary>
        /// <param name="adminpassword">string - Current Global Admin Password</param>
        /// <returns>string - Successful update / Generated API Key</returns>

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GenerateAPIKey([BindRequired][FromHeader(Name = "AdminPassword")] string adminpassword)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                if (GeneralHelper.IsNotNull(adminpassword))
                {
                    if (adminpassword == GeneralHelper.DecryptString(_config.AdminPassword!, _config.SALTKey!))
                    {
                        serviceResponse = await _adminService.GenerateAPIKey();
                        return (serviceResponse.Success ? Ok("SUCCESS") : Ok("FAILURE - There was a problem generating the API Key."));
                    }
                    else
                    {
                        return Ok("FAILURE - Incorrect Global Admin Password.");
                    }

                }
                else
                {
                    return Ok("FAILURE - You must provide the Global Admin Password.");
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// This function will return the admin log data.
        /// </summary>
        /// <returns>json - Current admin log data</returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAdminLog([BindRequired][FromHeader(Name = "AdminPassword")] string adminpassword)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                if (GeneralHelper.IsNotNull(adminpassword))
                {
                    if (adminpassword == GeneralHelper.DecryptString(_config.AdminPassword!, _config.SALTKey!))
                    {
                        serviceResponse = await _adminLogService.GetItems();
                        if (serviceResponse.Success)
                        {
                            return Ok(serviceResponse.ResponseObject);
                        }
                        else
                        {
                            return BadRequest(serviceResponse.ResponseObject);
                        }
                    }
                    else
                    {
                        return Ok("FAILURE - Incorrect Global Admin Password.");
                    }

                }
                else
                {
                    return Ok("FAILURE - You must provide the Global Admin Password.");
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// This function will purge the admin log data.
        /// </summary>
        /// <returns>dttring - Successful operation</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PurgeAdminLog([BindRequired][FromHeader(Name = "AdminPassword")] string adminpassword)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                if (GeneralHelper.IsNotNull(adminpassword))
                {
                    if (adminpassword == GeneralHelper.DecryptString(_config.AdminPassword!, _config.SALTKey!))
                    {
                        serviceResponse = await _adminLogService.DeleteAllItems();
                        if (serviceResponse.Success)
                        {
                            return Ok(serviceResponse.ResponseObject);
                        }
                        else
                        {
                            return BadRequest(serviceResponse.ResponseObject);
                        }
                    }
                    else
                    {
                        return Ok("FAILURE - Incorrect Global Admin Password.");
                    }

                }
                else
                {
                    return Ok("FAILURE - You must provide the Global Admin Password.");
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// This function will return the generated names data.
        /// </summary>
        /// <returns>json - Current generated names data</returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetGeneratedNamesLog()
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _generatedNamesService.GetItems();
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

        /// <summary>
        /// This function will purge the generated names data.
        /// </summary>
        /// <returns>dttring - Successful operation</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PurgeGeneratedNamesLog([BindRequired][FromHeader(Name = "AdminPassword")] string adminpassword)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                if (GeneralHelper.IsNotNull(adminpassword))
                {
                    if (adminpassword == GeneralHelper.DecryptString(_config.AdminPassword!, _config.SALTKey!))
                    {
                        serviceResponse = await _generatedNamesService.DeleteAllItems();
                        if (serviceResponse.Success)
                        {
                            return Ok(serviceResponse.ResponseObject);
                        }
                        else
                        {
                            return BadRequest(serviceResponse.ResponseObject);
                        }
                    }
                    else
                    {
                        return Ok("FAILURE - Incorrect Global Admin Password.");
                    }

                }
                else
                {
                    return Ok("FAILURE - You must provide the Global Admin Password.");
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// This function will reset the site configuration. THIS CANNOT BE UNDONE!
        /// </summary>
        /// <returns>dttring - Successful operation</returns>
        [HttpPost]
        [Route("[action]")]
        public IActionResult ResetSiteConfiguration([BindRequired][FromHeader(Name = "AdminPassword")] string adminpassword)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                if (GeneralHelper.IsNotNull(adminpassword))
                {
                    if (adminpassword == GeneralHelper.DecryptString(_config.AdminPassword!, _config.SALTKey!))
                    {
                        if (ConfigurationHelper.ResetSiteConfiguration())
                        {
                            return Ok("Site configuration reset suceeded!");
                        }
                        else
                        {
                            return BadRequest("Site configuration reset failed!");
                        }
                    }
                    else
                    {
                        return Ok("FAILURE - Incorrect Global Admin Password.");
                    }
                }
                else
                {
                    return Ok("FAILURE - You must provide the Global Admin Password.");
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