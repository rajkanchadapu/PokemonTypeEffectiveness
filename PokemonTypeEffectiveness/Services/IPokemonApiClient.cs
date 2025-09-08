using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonTypeEffectiveness.Models;

namespace PokemonTypeEffectiveness.Services
{
    public interface IPokemonApiClient
    {
        Task<Pokemon?> GetPokemonAsync(string name);
        Task<TypeRelations?> GetTypeRelationsAsync(string typeName);
    }
    public interface ITypeEffectivenessService
    {
        Task<StrengthWeaknessResult> GetStrengthsAndWeaknessesAsync(string pokemonName);
    }
}
