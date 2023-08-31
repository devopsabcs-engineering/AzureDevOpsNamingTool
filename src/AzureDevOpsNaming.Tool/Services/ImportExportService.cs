using AzureNaming.Tool.Helpers;
using AzureNaming.Tool.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AzureNaming.Tool.Services
{
    public class ImportExportService : IImportExportService
    {
        private IGeneratedNamesService _generatedNamesService;
        private IAdminLogService _adminLogService;
        private IResourceComponentService _resourceComponentService;
        private IResourceDelimiterService _resourceDelimiterService;
        private IResourceEnvironmentService _resourceEnvironmentService;
        private IResourceFunctionService _resourceFunctionService;
        private IResourceLocationService _resourceLocationService;
        private IResourceOrgService _resourceOrgService;
        private IResourceProjAppSvcService _resourceProjAppSvcService;
        private IResourceTypeService _resourceTypeService;
        private IResourceUnitDeptService _resourceUnitDeptService;
        private ICustomComponentService _customComponentService;
        private IAdminUserService _adminUserService;

        public ImportExportService(IGeneratedNamesService generatedNamesService,
            IAdminLogService adminLogService,
            IResourceComponentService resourceComponentService,
            IResourceDelimiterService resourceDelimiterService,
            IResourceEnvironmentService resourceEnvironmentService,
            IResourceFunctionService resourceFunctionService,
            IResourceLocationService resourceLocationService,
            IResourceOrgService resourceOrgService,
            IResourceProjAppSvcService resourceProjAppSvcService,
            IResourceTypeService resourceTypeService,
            IResourceUnitDeptService resourceUnitDeptService,
            ICustomComponentService customComponentService,
            IAdminUserService adminUserService
            )
        {
            _generatedNamesService = generatedNamesService;
            _adminLogService = adminLogService;
            _resourceComponentService = resourceComponentService;
            _resourceDelimiterService = resourceDelimiterService;
            _resourceEnvironmentService = resourceEnvironmentService;
            _resourceFunctionService = resourceFunctionService;
            _resourceLocationService = resourceLocationService;
            _resourceOrgService = resourceOrgService;
            _resourceTypeService = resourceTypeService;
            _customComponentService = customComponentService;
            _resourceUnitDeptService = resourceUnitDeptService;
            _resourceProjAppSvcService = resourceProjAppSvcService;
            _adminUserService = adminUserService;
        }

        public async Task<ServiceResponse> ExportConfig(bool includeadmin = false)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                ConfigurationData configdata = new();
                // Get the current data
                //ResourceComponents
                serviceResponse = await _resourceComponentService.GetItems(true);
                if (serviceResponse.Success)
                {
                    if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                    {
                        configdata.ResourceComponents = serviceResponse.ResponseObject!;
                    }
                }

                //ResourceDelimiters
                serviceResponse = await _resourceDelimiterService.GetItems(true);
                if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                {
                    configdata.ResourceDelimiters = serviceResponse.ResponseObject!;
                }

                //ResourceEnvironments
                serviceResponse = await _resourceEnvironmentService.GetItems();
                if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                {
                    configdata.ResourceEnvironments = serviceResponse.ResponseObject!;
                }

                // ResourceFunctions
                serviceResponse = await _resourceFunctionService.GetItems();
                if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                {
                    configdata.ResourceFunctions = serviceResponse.ResponseObject!;
                }

                // ResourceLocations
                serviceResponse = await _resourceLocationService.GetItems();
                if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                {
                    configdata.ResourceLocations = serviceResponse.ResponseObject!;
                }

                // ResourceOrgs
                serviceResponse = await _resourceOrgService.GetItems();
                if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                {
                    configdata.ResourceOrgs = serviceResponse.ResponseObject!;
                }

                // ResourceProjAppSvc
                serviceResponse = await _resourceProjAppSvcService.GetItems();
                if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                {
                    configdata.ResourceProjAppSvcs = serviceResponse.ResponseObject!;
                }

                // ResourceTypes
                serviceResponse = await _resourceTypeService.GetItems();
                if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                {
                    configdata.ResourceTypes = serviceResponse.ResponseObject!;
                }

                // ResourceUnitDepts
                serviceResponse = await _resourceUnitDeptService.GetItems();
                if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                {
                    configdata.ResourceUnitDepts = serviceResponse.ResponseObject!;
                }

                // CustomComponents
                serviceResponse = await _customComponentService.GetItems();
                if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                {
                    configdata.CustomComponents = serviceResponse.ResponseObject!;
                }

                //GeneratedNames
                serviceResponse = await _generatedNamesService.GetItems();
                if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                {
                    configdata.GeneratedNames = serviceResponse.ResponseObject!;
                }

                //AdminLogs
                serviceResponse = await _adminLogService.GetItems();
                if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                {
                    configdata.AdminLogs = serviceResponse.ResponseObject;
                }

                // Get the current settings
                var config = ConfigurationHelper.GetConfigurationData();
                configdata.DismissedAlerts = config.DismissedAlerts;
                configdata.DuplicateNamesAllowed = config.DuplicateNamesAllowed;
                configdata.ConnectivityCheckEnabled = config.ConnectivityCheckEnabled;
                configdata.GenerationWebhook = config.GenerationWebhook;

                // Get the security settings
                if (includeadmin)
                {
                    configdata.SALTKey = config.SALTKey;
                    configdata.AdminPassword = config.AdminPassword;
                    configdata.APIKey = config.APIKey;
                    //IdentityHeaderName
                    configdata.IdentityHeaderName = config.IdentityHeaderName;
                    //AdminUsers
                    serviceResponse = await _adminUserService.GetItems();
                    if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                    {
                        configdata.AdminUsers = serviceResponse.ResponseObject!;
                    }
                    // ResourceTypeEditing
                    configdata.ResourceTypeEditingAllowed = config.ResourceTypeEditingAllowed;
                }

                serviceResponse.ResponseObject = configdata;
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                serviceResponse.Success = false;
                serviceResponse.ResponseObject = ex;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse> PostConfig(ConfigurationData configdata)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Write all the configurations
                await _resourceComponentService.PostConfig(configdata.ResourceComponents);
                await _resourceDelimiterService.PostConfig(configdata.ResourceDelimiters);
                await _resourceEnvironmentService.PostConfig(configdata.ResourceEnvironments);
                await _resourceFunctionService.PostConfig(configdata.ResourceFunctions);
                await _resourceLocationService.PostConfig(configdata.ResourceLocations);
                await _resourceOrgService.PostConfig(configdata.ResourceOrgs);
                await _resourceProjAppSvcService.PostConfig(configdata.ResourceProjAppSvcs);
                await _resourceTypeService.PostConfig(configdata.ResourceTypes);
                await _resourceUnitDeptService.PostConfig(configdata.ResourceUnitDepts);
                await _customComponentService.PostConfig(configdata.CustomComponents);
                await _generatedNamesService.PostConfig(configdata.GeneratedNames);
                if (GeneralHelper.IsNotNull(configdata.AdminUsers))
                {
                    await _adminUserService.PostConfig(configdata.AdminUsers);
                }
                if (GeneralHelper.IsNotNull(configdata.AdminLogs))
                {
                    await _adminLogService.PostConfig(configdata.AdminLogs);
                }

                var config = ConfigurationHelper.GetConfigurationData();
                config.DismissedAlerts = configdata.DismissedAlerts;
                config.DuplicateNamesAllowed = configdata.DuplicateNamesAllowed;
                config.ConnectivityCheckEnabled = configdata.ConnectivityCheckEnabled;

                // Set the admin settings, if they are included in the import
                if (GeneralHelper.IsNotNull(configdata.SALTKey))
                {
                    config.SALTKey = configdata.SALTKey;
                }
                if (GeneralHelper.IsNotNull(configdata.AdminPassword))
                {
                    config.AdminPassword = configdata.AdminPassword;
                }
                if (GeneralHelper.IsNotNull(configdata.APIKey))
                {
                    config.APIKey = configdata.APIKey;
                }
                if (GeneralHelper.IsNotNull(configdata.IdentityHeaderName))
                {
                    config.IdentityHeaderName = configdata.IdentityHeaderName;
                }
                if (GeneralHelper.IsNotNull(configdata.ResourceTypeEditingAllowed))
                {
                    config.ResourceTypeEditingAllowed = configdata.ResourceTypeEditingAllowed;
                }
                var jsonWriteOptions = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };
                jsonWriteOptions.Converters.Add(new JsonStringEnumConverter());

                var newJson = JsonSerializer.Serialize(config, jsonWriteOptions);

                var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings/appsettings.json");
                File.WriteAllText(appSettingsPath, newJson);
                CacheHelper.ClearAllCache();
                serviceResponse.Success = true;
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
