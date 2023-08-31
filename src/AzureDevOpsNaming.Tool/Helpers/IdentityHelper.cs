using AzureNaming.Tool.Models;
using AzureNaming.Tool.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace AzureNaming.Tool.Helpers
{
    public class IdentityHelper
    {
        private static IAdminUserService _adminUserService;
        private static IAdminLogService _adminLogService;

        public IdentityHelper(IAdminUserService adminUserService,
            IAdminLogService adminLogService)
        {
            _adminLogService = adminLogService;
            _adminUserService = adminUserService;
        }

        public static async Task<bool> IsAdminUser(StateContainer state, ProtectedSessionStorage session, string name)
        {
            bool result = false;
            try
            {
                // Check if the username is in the list of Admin Users
                ServiceResponse serviceResponse = await _adminUserService.GetItems();
                if (serviceResponse.Success)
                {
                    if (GeneralHelper.IsNotNull(serviceResponse.ResponseObject))
                    {
                        List<AdminUser> adminusers = serviceResponse.ResponseObject!;
                        if (adminusers.Exists(x => x.Name.ToLower() == name.ToLower()))
                        {
                            state.SetAdmin(true);
                            await session.SetAsync("admin", true);
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
            }
            return result;
        }

        public static async Task<string> GetCurrentUser(ProtectedSessionStorage session)
        {
            string currentuser = "System";
            try
            {
                var currentuservalue = await session.GetAsync<string>("currentuser");
                if (!String.IsNullOrEmpty(currentuservalue.Value))
                {
                    currentuser = currentuservalue.Value;
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
            }
            return currentuser;
        }
    }
}
