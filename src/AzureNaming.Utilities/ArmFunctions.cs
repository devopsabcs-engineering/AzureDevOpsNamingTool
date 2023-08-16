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

        public static string? Base64(params string[] values)
        {
            var parameters = values.Select(
                arg => new FunctionArgument(
                    JToken.FromObject(arg)
                )
            ).ToArray();
            var result = ExpressionBuiltInFunctions.Functions
                .EvaluateFunction("base64", parameters, null);
            return result.Value<string>();
        }

        public static Newtonsoft.Json.Linq.JObject? Base64ToJson(params string[] values)
        {
            var parameters = values.Select(
                arg => new FunctionArgument(
                    JToken.FromObject(arg)
                )
            ).ToArray();
            var result = ExpressionBuiltInFunctions.Functions
                .EvaluateFunction("base64ToJson", parameters, null);
            return result.Value<Newtonsoft.Json.Linq.JObject>();
        }

        public static string? Base64ToString(params string[] values)
        {
            var parameters = values.Select(
                arg => new FunctionArgument(
                    JToken.FromObject(arg)
                )
            ).ToArray();
            var result = ExpressionBuiltInFunctions.Functions
                .EvaluateFunction("base64ToString", parameters, null);
            return result.Value<string>();
        }
    }
}