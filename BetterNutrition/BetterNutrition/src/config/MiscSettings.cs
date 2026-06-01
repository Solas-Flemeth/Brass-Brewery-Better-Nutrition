using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BetterNutrition;

public class MiscSettings
{
    [Description("Toggles whether the vanilla 20% Offhand Hunger Penalty should apply. Set to false to have no penalty for having an item in your offhand")]
    public bool OffHandHungerPenalty { get; set; } = false;
    
    [Range(-1000f, 9500f), DefaultValue(500f)]
    [Description("How much satiety is added to the games base satiety. This is compatible with XSkill's Huge Stomach.")]
    public float AdditionalSatietyBonus { get; set; } = 500f;

    [Range(0.0f, 20f), DefaultValue(2.5f)] 
    [Description("Override the damage of the vanilla starvation system. Great for saving your ears from the annoying starvation horn sounds. Vanilla has a default value of '0.125'.")]
    public float VanillaStarvationDamage { get; set; } = 2.5f;
}