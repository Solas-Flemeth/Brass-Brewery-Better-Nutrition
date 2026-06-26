using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BetterNutrition;

public class StarvationSettings
{
    [Description("The player stats will change as their satiety bar lowers once at the Minimum Satiety Starvation. Impacts must have their equivelant food nutrition impact enabled to apply")]
    public bool Enable { get; set; } = true;
    [Description("Modify the look of the hungerbar as starvation starts to kick in")]
    public bool HungerbarChange { get; set; } = true;
    [DisplayFormat(DataFormatString = "P"), Range(0.0f, 1.0f), DefaultValue(0.5f)]
    [Description("What is the mininmum percentage of satiety missing that starvation mode begins to start")]
    public float MinimumFoodToStarve
    {
        get;
        set => field = Math.Clamp(value, 0.0f, 1.0f);
    } = 0.5f;
}