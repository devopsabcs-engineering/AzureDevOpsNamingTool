using Azure.Deployments.Expression.Expressions;
using Newtonsoft.Json.Linq;

namespace AzureNaming.Utilities
{
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