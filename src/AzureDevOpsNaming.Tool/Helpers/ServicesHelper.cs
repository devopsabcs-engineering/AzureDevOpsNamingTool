using AzureNaming.Tool.Models;
using AzureNaming.Tool.Services;

namespace AzureNaming.Tool.Helpers
{
    public class ServicesHelper
    {
        private static IResourceComponentService _resourceComponentService;
        private static IResourceDelimiterService _resourceDelimiterService;
        private static IResourceEnvironmentService _resourceEnvironmentService;
        private static IResourceLocationService _resourceLocationService;
        private static IResourceOrgService _resourceOrgService;
        private static IResourceProjAppSvcService _resourceProjAppSvcService;
        private static IResourceTypeService _resourceTypeService;
        private static IResourceUnitDeptService _resourceUnitDeptService;
        private static IResourceFunctionService _resourceFunctionService;
        private static ICustomComponentService _customComponentService;
        private static IGeneratedNamesService _generatedNamesService;
        private static IAdminLogService _adminLogService;
        private static IAdminUserService _adminUserService;

        public ServicesHelper(IResourceComponentService resourceComponentService,
            IResourceDelimiterService resourceDelimiterService,
            IResourceEnvironmentService resourceEnvironmentService,
             IResourceLocationService resourceLocationService,
             IResourceOrgService resourceOrgService,
             IResourceProjAppSvcService resourceProjAppSvcService,
             IResourceTypeService resourceTypeService,
             IResourceUnitDeptService resourceUnitDeptService,
             IResourceFunctionService resourceFunctionService,
             ICustomComponentService customComponentService,
             IGeneratedNamesService generatedNamesService,
             IAdminLogService adminLogService,
             IAdminUserService adminUserService
            )
        {
            _resourceComponentService = resourceComponentService;
            _resourceDelimiterService = resourceDelimiterService;
            _resourceEnvironmentService = resourceEnvironmentService;
            _resourceLocationService = resourceLocationService;
            _resourceOrgService = resourceOrgService;
            _resourceProjAppSvcService = resourceProjAppSvcService;
            _resourceTypeService = resourceTypeService;
            _resourceUnitDeptService = resourceUnitDeptService;
            _resourceFunctionService = resourceFunctionService;
            _customComponentService = customComponentService;
            _generatedNamesService = generatedNamesService;
            _adminLogService = adminLogService;
            _adminUserService = adminUserService;
        }

        public static async Task<ServicesData> LoadServicesData(ServicesData servicesData, bool admin)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                serviceResponse = await _resourceComponentService.GetItems(admin);
                servicesData.ResourceComponents = (List<ResourceComponent>?)serviceResponse.ResponseObject;
                serviceResponse = await _resourceDelimiterService.GetItems(admin);
                servicesData.ResourceDelimiters = (List<ResourceDelimiter>?)serviceResponse.ResponseObject;
                serviceResponse = await _resourceEnvironmentService.GetItems();
                servicesData.ResourceEnvironments = (List<ResourceEnvironment>?)serviceResponse.ResponseObject;
                serviceResponse = await _resourceLocationService.GetItems(admin);
                servicesData.ResourceLocations = (List<ResourceLocation>?)serviceResponse.ResponseObject;
                serviceResponse = await _resourceOrgService.GetItems();
                servicesData.ResourceOrgs = (List<ResourceOrg>?)serviceResponse.ResponseObject;
                serviceResponse = await _resourceProjAppSvcService.GetItems();
                servicesData.ResourceProjAppSvcs = (List<ResourceProjAppSvc>?)serviceResponse.ResponseObject;
                serviceResponse = await _resourceTypeService.GetItems(admin);
                servicesData.ResourceTypes = (List<ResourceType>?)serviceResponse.ResponseObject;
                serviceResponse = await _resourceUnitDeptService.GetItems();
                servicesData.ResourceUnitDepts = (List<ResourceUnitDept>?)serviceResponse.ResponseObject;
                serviceResponse = await _resourceFunctionService.GetItems();
                servicesData.ResourceFunctions = (List<ResourceFunction>?)serviceResponse.ResponseObject;
                serviceResponse = await _customComponentService.GetItems();
                servicesData.CustomComponents = (List<CustomComponent>?)serviceResponse.ResponseObject;
                serviceResponse = await _generatedNamesService.GetItems();
                servicesData.GeneratedNames = (List<GeneratedName>?)serviceResponse.ResponseObject;
                serviceResponse = await _adminLogService.GetItems();
                servicesData.AdminLogMessages = (List<AdminLogMessage>?)serviceResponse.ResponseObject;
                serviceResponse = await _adminUserService.GetItems();
                servicesData.AdminUsers = (List<AdminUser>?)serviceResponse.ResponseObject;
                return servicesData;
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return servicesData;
            }
        }
    }
}
