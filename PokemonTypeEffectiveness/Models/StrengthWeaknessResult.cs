using System.Collections.Generic;

namespace PokemonTypeEffectiveness.Models
{
    public class StrengthWeaknessResult
    {
        public List<string> Strengths { get; set; } = new();
        public List<string> Weaknesses { get; set; } = new();
    }
}
