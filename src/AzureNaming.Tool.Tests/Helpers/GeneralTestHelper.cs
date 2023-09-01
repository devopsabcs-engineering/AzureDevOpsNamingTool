using AzureNaming.Tool.Models;
using System.Text.Json;

namespace AzureNaming.Tool.Helpers
{
    public static class GeneralTestHelper

    {

        #region Helpers

        public static string RemoveComponentFromCsv(string optionalCsv, string componentToRemove)
        {
            // essentially the same implementation in actual code
            string newOptionalCsv = optionalCsv;
            var currentvalues = new List<string>(optionalCsv.Split(','));
            if (currentvalues.Contains(componentToRemove))
            {
                currentvalues.Remove(componentToRemove);
                newOptionalCsv = String.Join(",", currentvalues.ToArray());
            }
            return newOptionalCsv;
        }

        public static T DeserializeJsonFromFile<T>(string fileName)
        {
            string jsonString = File.ReadAllText(fileName);
            var obj = JsonSerializer.Deserialize<T>(jsonString)!;
            return obj;
        }

        public static Task<ServiceResponse> GetSampleResourceTypeServiceResponseAsTask()
        {
            var serviceResponse = new ServiceResponse()
            {
                ResponseMessage = "",
                Success = true,
                ResponseObject = new List<ResourceType>()
                {
                    new ResourceType(){
                        Id = 1,
                        Resource = "AnalysisServices/servers",
                        Optional = "UnitDept,UniqueString",
                        Exclude = "Org,Function",
                        Property = "",
                        ShortName = "as",
                        Scope = "resource group",
                        LengthMin = "3",
                        LengthMax = "63",
                        ValidText = "Lowercase letters and numbers. Start with lowercase letter.",
                        InvalidText = "",
                        InvalidCharacters = "",
                        InvalidCharactersStart = "",
                        InvalidCharactersEnd = "",
                        InvalidCharactersConsecutive = "",
                        Regx = "^[a-z][a-z0-9]{2,62}$",
                        StaticValues = "",
                        Enabled = true
                    }
                }
            };

            // manually wrap the serviceResponse in a new Task
            return new Task<ServiceResponse>(() => { return serviceResponse; });
        }

        #endregion Helpers
    }
}
