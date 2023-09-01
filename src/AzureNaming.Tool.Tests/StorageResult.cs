using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace AzureNaming.Tool
{
    public readonly struct StorageResult<TValue>
    {
        /// <summary>
        /// Gets whether the operation succeeded.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets the result value of the operation.
        /// </summary>
        public TValue? Value { get; }

        public StorageResult(bool success, TValue? value)
        {
            Success = success;
            Value = value;
        }

        public StorageResult(in ProtectedBrowserStorageResult<TValue> result)
        {
            Success = result.Success;
            Value = result.Value;
        }
    }
}
