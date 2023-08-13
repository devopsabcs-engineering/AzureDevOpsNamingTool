using Azure.Deployments.Expression.Expressions;
using Newtonsoft.Json.Linq;

namespace AzureNaming.Utilities
{
    //https://samcogan.com/using-bicep-functions-in-c-if-you-really-want-to/
    //https://stackoverflow.com/questions/43295720/azure-arm-uniquestring-function-mimic/67399362#67399362
    public static class ArmFunctions
    {
        public static string? UniqueString(params string[] values)
        {
            var parameters = values.Select(
                arg => new FunctionArgument(
                    JToken.FromObject(arg)
                )
            ).ToArray();
            var result = ExpressionBuiltInFunctions.Functions
                .EvaluateFunction("uniqueString", parameters, null);
            return result.Value<string>();
        }
    }
}