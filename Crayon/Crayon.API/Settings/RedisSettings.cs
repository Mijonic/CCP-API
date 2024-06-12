namespace Crayon.API.Settings
{
    public class RedisSettings
    {
        public const string RedisSettingsName = "Redis";

        public string ConnectionString { get; set; }   
        public string Version { get; set; }
        public CacheTime CacheTime { get; set; }


    }

    public class CacheTime
    {
        public int UserAccounts { get; set; }
        public int AvailableServices { get; set; }
    }
}
