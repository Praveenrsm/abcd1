namespace FarmApi.Model
{
    public class CacheEntry<T>
    {
        public T Data { get; set; }
        public DateTimeOffset ExpiryTime { get; set; }
    }
}
