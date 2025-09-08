using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using PokemonTypeEffectiveness.Models;
using PokemonTypeEffectiveness.Services;
using Xunit;

namespace PokemonTypeEffectiveness.Tests
{
    public class TypeEffectivenessServiceTests
    {
        [Fact]
        public async Task SingleType_ReturnsExpectedStrengths()
        {
            var mockClient = new Mock<IPokemonApiClient>();
            mockClient.Setup(m => m.GetPokemonAsync("pikachu"))
                .ReturnsAsync(new Pokemon
                {
                    Types = new List<PokemonTypeSlot>
                    {
                        new PokemonTypeSlot { Slot = 1, TypeInfo = new TypeInfo { Name = "electric" } }
                    }
                });

            mockClient.Setup(m => m.GetTypeRelationsAsync("electric"))
                .ReturnsAsync(new TypeRelations
                {
                    DamageRelations = new DamageRelations
                    {
                        DoubleDamageTo = new List<TypeInfo> { new TypeInfo { Name = "water" } },
                        NoDamageFrom = new List<TypeInfo> { new TypeInfo { Name = "ground" } },
                        HalfDamageFrom = new List<TypeInfo> { new TypeInfo { Name = "steel" } }
                    }
                });

            var service = new TypeEffectivenessService(mockClient.Object);

            var result = await service.GetStrengthsAndWeaknessesAsync("pikachu");

            Assert.Contains("water", result.Strengths);
            Assert.Contains("ground", result.Strengths);
            Assert.Contains("steel", result.Strengths);
            Assert.Empty(result.Weaknesses);
        }

        [Fact]
        public async Task MultiType_MergesStrengthsAndWeaknesses()
        {
            var mockClient = new Mock<IPokemonApiClient>();
            mockClient.Setup(m => m.GetPokemonAsync("dualmon"))
                .ReturnsAsync(new Pokemon
                {
                    Types = new List<PokemonTypeSlot>
                    {
                        new PokemonTypeSlot { Slot = 1, TypeInfo = new TypeInfo { Name = "normal" } },
                        new PokemonTypeSlot { Slot = 2, TypeInfo = new TypeInfo { Name = "ghost" } }
                    }
                });

            mockClient.Setup(m => m.GetTypeRelationsAsync("normal"))
                .ReturnsAsync(new TypeRelations
                {
                    DamageRelations = new DamageRelations
                    {
                        NoDamageTo = new List<TypeInfo> { new TypeInfo { Name = "ghost" } },
                        HalfDamageTo = new List<TypeInfo> { new TypeInfo { Name = "rock" } }
                    }
                });

            mockClient.Setup(m => m.GetTypeRelationsAsync("ghost"))
                .ReturnsAsync(new TypeRelations
                {
                    DamageRelations = new DamageRelations
                    {
                        DoubleDamageTo = new List<TypeInfo> { new TypeInfo { Name = "psychic" } },
                        DoubleDamageFrom = new List<TypeInfo> { new TypeInfo { Name = "dark" } },
                        NoDamageFrom = new List<TypeInfo> { new TypeInfo { Name = "normal" } }
                    }
                });

            var service = new TypeEffectivenessService(mockClient.Object);

            var result = await service.GetStrengthsAndWeaknessesAsync("dualmon");

            Assert.Contains("psychic", result.Strengths);
            Assert.Contains("normal", result.Strengths);

            Assert.Contains("ghost", result.Weaknesses);
            Assert.Contains("rock", result.Weaknesses);
            Assert.Contains("dark", result.Weaknesses);
        }

        [Fact]
        public async Task InvalidPokemon_ThrowsException()
        {
            var mockClient = new Mock<IPokemonApiClient>();
            mockClient.Setup(m => m.GetPokemonAsync("invalid")).ThrowsAsync(new System.Exception("Pokemon 'invalid' not found."));

            var service = new TypeEffectivenessService(mockClient.Object);

            await Assert.ThrowsAsync<System.Exception>(() => service.GetStrengthsAndWeaknessesAsync("invalid"));
        }
    }
}
