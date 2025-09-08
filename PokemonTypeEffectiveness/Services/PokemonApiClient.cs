using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PokemonTypeEffectiveness.Models;

namespace PokemonTypeEffectiveness.Services
{
    public class PokemonApiClient : IPokemonApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public PokemonApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Pokemon?> GetPokemonAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("name required", nameof(name));

            var resp = await _httpClient.GetAsync($"pokemon/{name.ToLowerInvariant().Trim()}");
            
            if (!resp.IsSuccessStatusCode)
            {
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new Exception($"Pokemon '{name}' not found.");
                throw new Exception($"Error fetching Pokemon '{name}': {resp.ReasonPhrase}");
            }

            var stream = await resp.Content.ReadAsStreamAsync();
            var pokemon = await JsonSerializer.DeserializeAsync<Pokemon>(stream, _options);
            if (pokemon == null) throw new Exception("Empty response when fetching Pokemon.");
            return pokemon;
        }

        public async Task<TypeRelations?> GetTypeRelationsAsync(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName)) throw new ArgumentException("typeName required", nameof(typeName));
            var resp = await _httpClient.GetAsync($"type/{typeName.ToLowerInvariant().Trim()}");
            if (!resp.IsSuccessStatusCode)
            {
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new Exception($"Type '{typeName}' not found.");
                throw new Exception($"Error fetching type '{typeName}': {resp.ReasonPhrase}");
            }

            await using var stream = await resp.Content.ReadAsStreamAsync();
            var typeRelations = await JsonSerializer.DeserializeAsync<TypeRelations>(stream, _options);
            if (typeRelations == null) throw new Exception($"Empty response when fetching type '{typeName}'.");
            return typeRelations;
        }
    }
}
