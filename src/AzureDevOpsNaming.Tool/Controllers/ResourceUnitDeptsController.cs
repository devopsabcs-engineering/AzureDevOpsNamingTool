﻿using AzureNaming.Tool.Attributes;
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
    public class ResourceUnitDeptsController : ControllerBase
    {
        private readonly IResourceUnitDeptService _resourceUnitDeptService;
        private readonly IAdminLogService _adminLogService;

        public ResourceUnitDeptsController(
            IResourceUnitDeptService resourceUnitDeptService,
            IAdminLogService adminLogService)
        {
            _resourceUnitDeptService = resourceUnitDeptService;
            _adminLogService = adminLogService;
        }

        // GET: api/<ResourceUnitDeptsController>
        /// <summary>
        /// This function will return the units/depts data. 
        /// </summary>
        /// <returns>json - Current units/depts data</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                serviceResponse = await _resourceUnitDeptService.GetItems();
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

        // GET api/<ResourceUnitDeptsController>/5
        /// <summary>
        /// This function will return the specifed unit/dept data.
        /// </summary>
        /// <param name="id">int - Unit/Dept id</param>
        /// <returns>json - Unit/Dept data</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                serviceResponse = await _resourceUnitDeptService.GetItem(id);
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

        // POST api/<ResourceUnitDeptsController>
        /// <summary>
        /// This function will create/update the specified unit/dept data.
        /// </summary>
        /// <param name="item">ResourceUnitDept (json) - Unit/Dept data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResourceUnitDept item)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceUnitDeptService.PostItem(item);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Unit/Department (" + item.Name + ") added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceUnitDept");
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

        // POST api/<ResourceUnitDeptsController>
        /// <summary>
        /// This function will update all units/depts data.
        /// </summary>
        /// <param name="items">List - ResourceUnitDept (json) - All units/depts data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceUnitDept> items)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceUnitDeptService.PostConfig(items);
                if (serviceResponse.Success)
                {
                    _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Units/Departments added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceUnitDept");
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

        // DELETE api/<ResourceUnitDeptsController>/5
        /// <summary>
        /// This function will delete the specifed unit/dept data.
        /// </summary>
        /// <param name="id">int - Unit/Dept id</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get the item details
                serviceResponse = await _resourceUnitDeptService.GetItem(id);
                if (serviceResponse.Success)
                {
                    ResourceUnitDept item = (ResourceUnitDept)serviceResponse.ResponseObject!;
                    serviceResponse = await _resourceUnitDeptService.DeleteItem(id);
                    if (serviceResponse.Success)
                    {
                        _adminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Unit/Department (" + item.Name + ") deleted." });
                        CacheHelper.InvalidateCacheObject("ResourceUnitDept");
                        return Ok("Resource Unit/Department (" + item.Name + ") deleted.");
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