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


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzureNaming.Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class ResourceProjAppSvcsController : ControllerBase
    {
        // GET: api/<ResourceProjAppSvcsController>
        /// <summary>
        /// This function will return the projects/apps/services data. 
        /// </summary>
        /// <returns>json - Current projects/apps/servicse data</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await ResourceProjAppSvcService.GetItems();
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

        // GET api/<ResourceProjAppSvcsController>/5
        /// <summary>
        /// This function will return the specifed project/app/service data.
        /// </summary>
        /// <param name="id">int - Project/App/Service id</param>
        /// <returns>json - Project/App/Service data</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                serviceResponse = await ResourceProjAppSvcService.GetItem(id);
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

        // POST api/<ResourceProjAppSvcsController>
        /// <summary>
        /// This function will create/update the specified project/app/service data.
        /// </summary>
        /// <param name="item">ResourceProjAppSvc (json) - Project/App/Service data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResourceProjAppSvc item)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await ResourceProjAppSvcService.PostItem(item);
                if (serviceResponse.Success)
                {
                    AdminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Project/App/Service (" + item.Name + ") added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceProjAppSvc");
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

        // POST api/<ResourceProjAppSvcsController>
        /// <summary>
        /// This function will update all projects/apps/services data.
        /// </summary>
        /// <param name="items">List - ResourceProjAppSvc (json) - All projects/apps/services data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceProjAppSvc> items)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await ResourceProjAppSvcService.PostConfig(items);
                if (serviceResponse.Success)
                {
                    AdminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Projects/Apps/Services added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceProjAppSvc");
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

        // DELETE api/<ResourceProjAppSvcsController>/5
        /// <summary>
        /// This function will delete the specifed project/app/service data.
        /// </summary>
        /// <param name="id">int - Project/App?service id</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get the item details
                serviceResponse = await ResourceProjAppSvcService.GetItem(id);
                if (serviceResponse.Success)
                {
                    ResourceProjAppSvc item = (ResourceProjAppSvc)serviceResponse.ResponseObject!;
                    serviceResponse = await ResourceProjAppSvcService.DeleteItem(id);
                    if (serviceResponse.Success)
                    {
                        AdminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Project/App/Service (" + item.Name + ") deleted." });
                        CacheHelper.InvalidateCacheObject("ResourceProjAppSvc");
                        return Ok("Resource Project/App/Service (" + item.Name + ") deleted.");
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
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }
    }
}