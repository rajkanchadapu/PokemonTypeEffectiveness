using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonTypeEffectiveness.Models;

namespace PokemonTypeEffectiveness.Services
{
    public class TypeEffectivenessService: ITypeEffectivenessService
    {
        private readonly IPokemonApiClient  _client;
        public TypeEffectivenessService(IPokemonApiClient client)
        {
            _client = client;
        }
        public async Task<StrengthWeaknessResult> GetStrengthsAndWeaknessesAsync(string pokemonName)
        {
            if (string.IsNullOrWhiteSpace(pokemonName)) 
                throw new ArgumentException("pokemonName required", nameof(pokemonName));

            var pokemon = await _client.GetPokemonAsync(pokemonName);

            if (pokemon == null) 
                throw new Exception("Pokemon data not found.");
            
            if (pokemon.Types == null || pokemon.Types.Count == 0)
                throw new Exception("Pokemon has no types.");

            var strengths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var weaknesses = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var typeRelationTasks = pokemon.Types
                .Where(slot => !string.IsNullOrWhiteSpace(slot?.TypeInfo?.Name))
                .Select(async slot =>
            {
                var typeName = slot.TypeInfo.Name; 
                return await _client.GetTypeRelationsAsync(typeName);
            });

            var typeRelations = await Task.WhenAll(typeRelationTasks);

            foreach (var relations in typeRelations)
            {
                var damageRelations = relations?.DamageRelations;
                if (damageRelations == null) continue;
                // Strong: double_damage_to, no_damage_from, half_damage_from
                AddDamageTypes(damageRelations.DoubleDamageTo, strengths);
                AddDamageTypes(damageRelations.NoDamageFrom, strengths);
                AddDamageTypes(damageRelations.HalfDamageFrom, strengths);
                // Weak: no_damage_to, half_damage_to, double_damage_from
                AddDamageTypes(damageRelations.NoDamageTo, weaknesses);
                AddDamageTypes(damageRelations.HalfDamageTo, weaknesses);
                AddDamageTypes(damageRelations.DoubleDamageFrom, weaknesses);
            } 

            return new StrengthWeaknessResult
            {
                Strengths = strengths.OrderBy(x => x).ToList(),
                Weaknesses = weaknesses.OrderBy(x => x).ToList()
            };
        }

        private void AddDamageTypes(IEnumerable<TypeInfo>? items, HashSet<string> set)
        {
            if (items is null) return;
            foreach (var item in items)
            {
                if (item?.Name is null) continue;
                set.Add(item.Name);
            }
        }
    }
}
