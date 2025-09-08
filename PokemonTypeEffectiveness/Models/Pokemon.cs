using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PokemonTypeEffectiveness.Models
{
    public class Pokemon
    {
        [JsonPropertyName("types")]
        public List<PokemonTypeSlot> Types { get; set; } = new();
    }

    public class PokemonTypeSlot
    {
        [JsonPropertyName("slot")]
        public int Slot { get; set; }

        [JsonPropertyName("type")]
        public TypeInfo TypeInfo { get; set; } = new();
    }
}
