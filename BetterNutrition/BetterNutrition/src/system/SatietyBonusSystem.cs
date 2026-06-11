using System;
using Vintagestory;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace BetterNutrition;

public class SatietyBonusSystem
{
    private static readonly BetterNutritionConfigData _config = BetterNutritionConfig.Config;
    
    public static void OnPlayerJoin(IServerPlayer player)
    {
        EntityBehaviorHunger? hunger = player.Entity?.GetBehavior<EntityBehaviorHunger>();
        if (hunger == null)
        {
            return;
        }
        float bonusSaturation = hunger.entity.Attributes.GetFloat("BrassBreweryBetterNutritionBonusSatiety");
        if (bonusSaturation != _config.Misc.AdditionalSatietyBonus)
        {
            
            float oldSaturation = hunger.MaxSaturation;
            hunger.MaxSaturation = hunger.MaxSaturation - bonusSaturation + _config.Misc.AdditionalSatietyBonus;
            if (hunger.Saturation >= hunger.MaxSaturation)
            {
                hunger.Saturation = hunger.MaxSaturation;
            }

            float saturationChangeCoefficent = hunger.MaxSaturation / oldSaturation;
            hunger.Saturation = Math.Clamp(hunger.Saturation * saturationChangeCoefficent, 0f, hunger.MaxSaturation);
            hunger.FruitLevel = Math.Clamp(hunger.FruitLevel * saturationChangeCoefficent, 0f, hunger.MaxSaturation);
            hunger.GrainLevel = Math.Clamp(hunger.GrainLevel * saturationChangeCoefficent, 0f, hunger.MaxSaturation);
            hunger.VegetableLevel = Math.Clamp(hunger.VegetableLevel * saturationChangeCoefficent, 0f, hunger.MaxSaturation);
            hunger.ProteinLevel = Math.Clamp(hunger.ProteinLevel * saturationChangeCoefficent, 0f, hunger.MaxSaturation);
            hunger.DairyLevel = Math.Clamp(hunger.DairyLevel * saturationChangeCoefficent, 0f, hunger.MaxSaturation);
            hunger.entity.Attributes.SetFloat("BrassBreweryBetterNutritionBonusSatiety", _config.Misc.AdditionalSatietyBonus);
            hunger.UpdateNutrientHealthBoost();
        }
    }
}   