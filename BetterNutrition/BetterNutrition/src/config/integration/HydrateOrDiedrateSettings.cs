using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BetterNutrition;

public class HydrateOrDiedrateSettings
{
    public bool Enable { get; set; } = true;
    public ThirstRateSettings ThirstRate { get; set; } = new ThirstRateSettings();
    public CoolingMultiplierSettings CoolingMultiplier { get; set; } = new CoolingMultiplierSettings();
}

public class CoolingMultiplierSettings: INutritionModifier
{
    [Description("Should the player have heat resistance when in good nutrition")]
    public bool Enable { get; set; } = true;
    
    [DisplayFormat(DataFormatString = "P"),Range(-0.25f, 0.25f), DefaultValue(0.03f)]
    public float Fruit { get; set; } = 0.03f;
    
    [DisplayFormat(DataFormatString = "P"),Range(-0.25f, 0.25f), DefaultValue(0.03f)]
    public float Grain { get; set; } = 0.03f;
    
    [DisplayFormat(DataFormatString = "P"),Range(-0.25f, 0.25f), DefaultValue(0.03f)]
    public float Vegetable { get; set; } = 0.03f;
    
    [DisplayFormat(DataFormatString = "P"),Range(-0.25f, 0.25f), DefaultValue(0.03f)]
    public float Protein { get; set; } = 0.03f;
    
    [DisplayFormat(DataFormatString = "P"),Range(-0.25f, 0.25f), DefaultValue(0.03f)]
    public float Dairy { get; set; } = 0.03f;
}

public class ThirstRateSettings: INutritionModifier
{
    [Description("Should the player have heat resistance when in good nutrition")]
    public bool Enable { get; set; } = true;
    
    [DisplayFormat(DataFormatString = "P"),Range(-0.25f, 0.25f), DefaultValue(0.03f)]
    public float Fruit { get; set; } = 0.03f;
    
    [DisplayFormat(DataFormatString = "P"),Range(-0.25f, 0.25f), DefaultValue(0.03f)]
    public float Grain { get; set; } = 0.03f;
    
    [DisplayFormat(DataFormatString = "P"),Range(-0.25f, 0.25f), DefaultValue(0.03f)]
    public float Vegetable { get; set; } = 0.03f;
    
    [DisplayFormat(DataFormatString = "P"),Range(-0.25f, 0.25f), DefaultValue(0.03f)]
    public float Protein { get; set; } = 0.03f;
    
    [DisplayFormat(DataFormatString = "P"),Range(-0.25f, 0.25f), DefaultValue(0.03f)]
    public float Dairy { get; set; } = 0.03f;
}