using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PokemonTypeEffectiveness.Services;

namespace PokemonTypeEffectiveness
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddHttpClient<IPokemonApiClient, PokemonApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddTransient<ITypeEffectivenessService, TypeEffectivenessService>();

            var provider = services.BuildServiceProvider();

            Console.WriteLine("Enter a Pokemon name:");
            var pokemonName = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                Console.WriteLine("Invalid input. Please enter a Pokemon name.");
                return;
            }

            var typeService = provider.GetRequiredService<ITypeEffectivenessService>();

            try
            {
                var result = await typeService.GetStrengthsAndWeaknessesAsync(pokemonName);

                Console.WriteLine($"\nResults for {pokemonName.ToUpperInvariant()}:");

                if (result.Strengths.Any())
                {
                    Console.WriteLine("\n Strengths:");
                    foreach (var strength in result.Strengths)
                        Console.WriteLine($" - {strength}");
                }
                else
                {
                    Console.WriteLine("\n Strengths: None found");
                }

                if (result.Weaknesses.Any())
                {
                    Console.WriteLine("\n Weaknesses:");
                    foreach (var weakness in result.Weaknesses)
                        Console.WriteLine($" - {weakness}");
                }
                else
                {
                    Console.WriteLine("\n Weaknesses: None found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
