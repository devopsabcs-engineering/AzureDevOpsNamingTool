﻿using AzureNaming.Tool.Helpers;
using AzureNaming.Tool.Models;
using System.Text.Json;

namespace AzureNaming.Tool.Services
{
    public class ResourceLocationService : IResourceLocationService
    {
        private IAdminLogService _adminLogService;

        public ResourceLocationService(
            IAdminLogService adminLogService)
        {
            _adminLogService = adminLogService;
        }
        public async Task<ServiceResponse> GetItems(bool admin = true)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                var items = await ConfigurationHelper.GetList<ResourceLocation>();
                if (GeneralHelper.IsNotNull(items))
                {
                    if (!admin)
                    {
                        serviceResponse.ResponseObject = items.Where(x => x.Enabled == true).OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        serviceResponse.ResponseObject = items.OrderBy(x => x.Name).ToList();
                    }
                    serviceResponse.Success = true;
                }
                else
                {
                    serviceResponse.ResponseObject = "Resource Locations not found!";
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                serviceResponse.Success = false;
                serviceResponse.ResponseObject = ex;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse> GetItem(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                var items = await ConfigurationHelper.GetList<ResourceLocation>();
                if (GeneralHelper.IsNotNull(items))
                {
                    var item = items.Find(x => x.Id == id);
                    if (GeneralHelper.IsNotNull(item))
                    {
                        serviceResponse.ResponseObject = item;
                        serviceResponse.Success = true;
                    }
                    else
                    {
                        serviceResponse.ResponseObject = "Resource Location not found!";
                    }
                }
                else
                {
                    serviceResponse.ResponseObject = "Resource Locations not found!";
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                serviceResponse.Success = false;
                serviceResponse.ResponseObject = ex;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse> PostItem(ResourceLocation item)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Make sure the new item short name only contains letters/numbers
                if (!ValidationHelper.CheckAlphanumeric(item.ShortName))
                {
                    serviceResponse.Success = false;
                    serviceResponse.ResponseObject = "Short name must be alphanumeric.";
                    return serviceResponse;
                }

                // Force lowercase on the shortname
                item.ShortName = item.ShortName.ToLower();

                // Get list of items
                var items = await ConfigurationHelper.GetList<ResourceLocation>();
                if (GeneralHelper.IsNotNull(items))
                {
                    // Set the new id
                    if (item.Id == 0)
                    {
                        if (items.Count > 0)
                        {
                            item.Id = items.Max(t => t.Id) + 1;
                        }
                        else
                        {
                            item.Id = 1;
                        }
                    }

                    // Determine new item id
                    if (items.Count > 0)
                    {
                        // Check if the item already exists
                        if (items.Exists(x => x.Id == item.Id))
                        {
                            // Remove the updated item from the list
                            var existingitem = items.Find(x => x.Id == item.Id);
                            if (GeneralHelper.IsNotNull(existingitem))
                            {
                                int index = items.IndexOf(existingitem);
                                items.RemoveAt(index);
                            }
                        }

                        // Put the item at the end
                        items.Add(item);
                    }
                    else
                    {
                        item.Id = 1;
                        items.Add(item);
                    }

                    // Write items to file
                    await ConfigurationHelper.WriteList<ResourceLocation>(items);
                    serviceResponse.Success = true;
                }
                else
                {
                    serviceResponse.ResponseObject = "Resource Locations not found!";
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                serviceResponse.ResponseObject = ex;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteItem(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                var items = await ConfigurationHelper.GetList<ResourceLocation>();
                if (GeneralHelper.IsNotNull(items))
                {
                    // Get the specified item
                    var item = items.Find(x => x.Id == id);
                    if (GeneralHelper.IsNotNull(item))
                    {
                        // Remove the item from the collection
                        items.Remove(item);

                        // Write items to file
                        await ConfigurationHelper.WriteList<ResourceLocation>(items);
                        serviceResponse.Success = true;
                    }
                    else
                    {
                        serviceResponse.ResponseObject = "Resource Location not found!";
                    }
                }
                else
                {
                    serviceResponse.ResponseObject = "Resource Locations not found!";
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                serviceResponse.ResponseObject = ex;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse> PostConfig(List<ResourceLocation> items)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                var newitems = new List<ResourceLocation>();
                int i = 1;

                // Determine new item id
                foreach (ResourceLocation item in items)
                {
                    // Make sure the new item short name only contains letters/numbers
                    if (!ValidationHelper.CheckAlphanumeric(item.ShortName))
                    {
                        serviceResponse.Success = false;
                        serviceResponse.ResponseObject = "Short name must be alphanumeric.";
                        return serviceResponse;
                    }

                    // Force lowercase on the shortname
                    item.ShortName = item.ShortName.ToLower();

                    item.Id = i;
                    newitems.Add(item);
                    i += 1;
                }

                // Write items to file
                await ConfigurationHelper.WriteList<ResourceLocation>(newitems);
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                serviceResponse.ResponseObject = ex;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse> RefreshResourceLocations(bool shortNameReset = false)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get the existing Resource location items
                serviceResponse = await this.GetItems();
                if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                {
                    List<ResourceLocation> locations = (List<ResourceLocation>)serviceResponse.ResponseObject!;
                    if (GeneralHelper.IsNotNull(locations))
                    {
                        string url = "https://raw.githubusercontent.com/devopsabcs-engineering/AzureDevOpsNamingTool/main/src/AzureDevOpsNaming.Tool/repository/resourcelocations.json";

                        string refreshdata = await GeneralHelper.DownloadString(url);
                        if (!String.IsNullOrEmpty(refreshdata))
                        {
                            var newlocations = new List<ResourceLocation>();
                            var options = new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                PropertyNameCaseInsensitive = true
                            };

                            newlocations = JsonSerializer.Deserialize<List<ResourceLocation>>(refreshdata, options);
                            if (GeneralHelper.IsNotNull(newlocations))
                            {
                                // Loop through the new items
                                // Add any new resource location and update any existing locations
                                foreach (ResourceLocation newlocation in newlocations)
                                {
                                    // Check if the existing locations contain the current location
                                    int i = locations.FindIndex(x => x.Name == newlocation.Name);
                                    if (i > -1)
                                    {
                                        // Update the Resource location Information
                                        ResourceLocation oldlocation = locations[i];
                                        newlocation.Enabled = oldlocation.Enabled;

                                        if ((!shortNameReset) || (String.IsNullOrEmpty(oldlocation.ShortName)))
                                        {
                                            newlocation.ShortName = oldlocation.ShortName;
                                        }
                                        // Remove the old location
                                        locations.RemoveAt(i);
                                        // Add the new location
                                        locations.Add(newlocation);
                                    }
                                    else
                                    {
                                        // Add a new resource location
                                        locations.Add(newlocation);
                                    }
                                }

                                // Update the settings file
                                serviceResponse = await PostConfig(locations);

                                // Update the repository file
                                await FileSystemHelper.WriteFile("resourcelocations.json", refreshdata, "repository/");

                                // Clear cached data
                                CacheHelper.InvalidateCacheObject("ResourceLocation");

                                // Update the current configuration file version data information
                                await ConfigurationHelper.UpdateConfigurationFileVersion("resourcelocations");
                            }
                            else
                            {
                                serviceResponse.ResponseObject = "Resource Locations not found!";
                            }
                        }
                        else
                        {
                            serviceResponse.ResponseObject = "Refresh Resource Locations not found!";
                        }
                    }
                    else
                    {
                        serviceResponse.ResponseObject = "Resource Locations not found!";
                    }
                }
                else
                {
                    serviceResponse.ResponseObject = "Resource Locations not found!";
                    _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = "There was a problem refreshing the resource locations configuration." });
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                serviceResponse.Success = false;
                serviceResponse.ResponseObject = ex;
            }
            return serviceResponse;
        }
    }
}
