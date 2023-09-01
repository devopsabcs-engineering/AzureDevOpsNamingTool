namespace AzureNaming.Tool
{
    public interface IBrowserStorage
    {
        ValueTask SetAsync(string key, object value);
        ValueTask SetAsync(string purpose, string key, object value);
        ValueTask<StorageResult<TValue>> GetAsync<TValue>(string key);
        ValueTask<StorageResult<TValue>> GetAsync<TValue>(string purpose, string key);
        ValueTask DeleteAsync(string key);
    }
}