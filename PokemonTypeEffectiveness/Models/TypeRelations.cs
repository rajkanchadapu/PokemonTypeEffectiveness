using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PokemonTypeEffectiveness.Models
{
    public class TypeRelations
    {
        [JsonPropertyName("damage_relations")]
        public DamageRelations DamageRelations { get; set; } = new();

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class DamageRelations
    {
        [JsonPropertyName("double_damage_to")]
        public List<TypeInfo> DoubleDamageTo { get; set; } = new();

        [JsonPropertyName("double_damage_from")]
        public List<TypeInfo> DoubleDamageFrom { get; set; } = new();

        [JsonPropertyName("half_damage_to")]
        public List<TypeInfo> HalfDamageTo { get; set; } = new();

        [JsonPropertyName("half_damage_from")]
        public List<TypeInfo> HalfDamageFrom { get; set; } = new();

        [JsonPropertyName("no_damage_to")]
        public List<TypeInfo> NoDamageTo { get; set; } = new();

        [JsonPropertyName("no_damage_from")]
        public List<TypeInfo> NoDamageFrom { get; set; } = new();
    }
}
