﻿using AzureNaming.Tool.Models;
using AzureNaming.Tool.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AzureNaming.Tool.Services;
using AzureNaming.Tool.Attributes;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzureNaming.Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class AdminController : ControllerBase
    {
        private readonly SiteConfiguration config = ConfigurationHelper.GetConfigurationData();

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
                    if (adminpassword == GeneralHelper.DecryptString(config.AdminPassword!, config.SALTKey!))
                    {
                        serviceResponse = await AdminService.UpdatePassword(password);
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
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
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
                    if (adminpassword == GeneralHelper.DecryptString(config.AdminPassword!, config.SALTKey!))
                    {
                        serviceResponse = await AdminService.UpdateAPIKey(apikey);
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
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
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
                    if (adminpassword == GeneralHelper.DecryptString(config.AdminPassword!, config.SALTKey!))
                    {
                        serviceResponse = await AdminService.GenerateAPIKey();
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
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
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
                    if (adminpassword == GeneralHelper.DecryptString(config.AdminPassword!, config.SALTKey!))
                    {
                        serviceResponse = await AdminLogService.GetItems();
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
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
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
                    if (adminpassword == GeneralHelper.DecryptString(config.AdminPassword!, config.SALTKey!))
                    {
                        serviceResponse = await AdminLogService.DeleteAllItems();
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
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
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
                serviceResponse = await GeneratedNamesService.GetItems();
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
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
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
                    if (adminpassword == GeneralHelper.DecryptString(config.AdminPassword!, config.SALTKey!))
                    {
                        serviceResponse = await GeneratedNamesService.DeleteAllItems();
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
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
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
                    if (adminpassword == GeneralHelper.DecryptString(config.AdminPassword!, config.SALTKey!))
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
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }
    }
}