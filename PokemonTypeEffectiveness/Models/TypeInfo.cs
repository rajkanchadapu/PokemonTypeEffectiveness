using System.Text.Json.Serialization;

namespace PokemonTypeEffectiveness.Models
{
    public class TypeInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}