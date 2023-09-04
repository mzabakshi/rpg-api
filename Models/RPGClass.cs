using System.Text.Json.Serialization;

namespace rpg_api.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RPGClass
    {
        Knight = 1,
        Mage = 2,
        Cleric = 3
    }
}