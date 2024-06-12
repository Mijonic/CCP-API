namespace Crayon.API.Settings
{
    public class DatabaseSettings
    {
        public const string DatabaseSettingsSettingsName = "Database";

        public string ConnectionString { get; set; }
        public PageSizes PageSizes { get; set; }


    }

    public class PageSizes
    {
        public int CustomerAccounts { get; set; }
        public int AvailableServices { get; set; }
        public int AccountLicences { get; set; }    
    }
}

