namespace Crayon.API.Settings
{
    public class CCPSettings
    {
        public const string CCPSettingsName = "CCP";

        public string Url { get; set; }
        public Endpoints Endpoints { get; set; }
        public CCPPageSizes PageSizes { get; set; }
    }

    public class Endpoints
    {
        public string GetAvailableServices { get; set; }
    }

    public class CCPPageSizes
    {
        public int AvailableServices { get; set; }
    }
}
