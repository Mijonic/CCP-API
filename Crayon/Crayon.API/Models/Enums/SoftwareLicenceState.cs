using System.Text.Json.Serialization;

namespace Crayon.API.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SoftwareLicenceState
    {
        Active = 0,
        Canceled = 1,
        Expired = 2
    }
}
