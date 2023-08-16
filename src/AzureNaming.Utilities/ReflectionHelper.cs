using System.Reflection;

namespace AzureNaming.Utilities
{
    public static class ReflectionHelper
    {
        public static object CallArmFunctionName(string methodName, object[] args)
        {
            Type type = typeof(ArmFunctions);
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            MethodInfo? info = type.GetMethod(
                methodName,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            if (info == null)
            {
                throw new ArgumentNullException($"Method {methodName} not found for type {type.FullName}");
            }

            object value = info.Invoke(null, args);

            return value;

        }
    }
}
