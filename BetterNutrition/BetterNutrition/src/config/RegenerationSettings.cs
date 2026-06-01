using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BetterNutrition;

public class RegenerationSettings
{
    [Description("Should player gain bonus regeneration for maintaining high satiety")]
    public bool Enable { get; set; } = true;
    
    [DisplayFormat(DataFormatString = "P"), Range(0.0f, 1.0f), DefaultValue(0.70f)]
    [Description("What is the mininmum amount of satiey required to have regeneration")]
    public float MinimumSatiety { get; set; } = 0.70f;

    [DisplayFormat(DataFormatString = "P"), Range(0.0f, 1.0f), DefaultValue(0.0025f)]
    [Description("What percentage of health is regenerated a heal tick")]
    public float RegenerationRate { get; set; } = 0.0025f;
    
    [Range(0f, 1f), DefaultValue(0.0125f)]
    [Description("What percentage of health should be regenerated a heal tick if sitting")]
    public float RestingRegenerationRate { get; set; } = 0.0125f;
    
    [Range(0.2f,10f), DefaultValue(1.0)]
    [Description("How often should the player have bonus regeneration in seconds")]
    public float TickRate { get; set; } = 1.0f;
    
}