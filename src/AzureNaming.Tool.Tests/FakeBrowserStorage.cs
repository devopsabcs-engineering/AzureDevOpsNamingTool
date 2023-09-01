namespace AzureNaming.Tool
{

    public class FakeBrowserStorage : IBrowserStorage
    {
        private Dictionary<(string Key, string Purpose), object> storage = new();

        public ValueTask DeleteAsync(string key)
        {
            storage.Remove((key, string.Empty));
            return ValueTask.CompletedTask;
        }

        public ValueTask<StorageResult<TValue>> GetAsync<TValue>(string key)
            => GetAsync<TValue>(string.Empty, key);

        public ValueTask<StorageResult<TValue>> GetAsync<TValue>(string purpose, string key)
        {
            var found = storage.TryGetValue((key, purpose), out var objValue);

            return found
                ? ValueTask.FromResult(new StorageResult<TValue>(found, (TValue)objValue))
                : ValueTask.FromResult(new StorageResult<TValue>(found, default(TValue)));
        }

        public ValueTask SetAsync(string key, object value)
            => SetAsync(string.Empty, key, value);

        public ValueTask SetAsync(string purpose, string key, object value)
        {
            storage.Add((key, purpose), value);
            return ValueTask.CompletedTask;
        }
    }
}
