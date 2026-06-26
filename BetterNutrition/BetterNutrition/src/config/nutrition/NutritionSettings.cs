using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BetterNutrition;

public class NutritionSettings
{
    [Description("Should the players stats change from their nutrition settings, including the starvation system and modded stats")]
    public bool Enable { get; set; } = true;
    
    [Range(0.1f, 600f), DefaultValue(3f)]
    [Description("How often should the player stats be updated in seconds")]
    public float UpdateFrequency { get; set; } = 3f;
    
    [DisplayFormat(DataFormatString = "P"), Range(0f, 0.5f), DefaultValue(0.02f)]
    [Description("What is the minimum % difference in nutrition or saturation required to update a stat")]
    public float UpdateThreshold { get; set; } = 0.02f;
    
    public WalkSpeedSettings WalkSpeed { get; set; } = new WalkSpeedSettings();
    public HealthSettings Health { get; set; } = new HealthSettings();
    public HungerRateSettings Hunger { get; set; } = new HungerRateSettings();
    public MeleeDamageSettings MeleeDamage { get; set; } = new MeleeDamageSettings();
    public MiningSpeedSettings MiningSpeed { get; set; } = new MiningSpeedSettings();
    public StarvationSettings Starvation { get; set; } = new StarvationSettings();
    
}

//sub classes
public class HungerRateSettings : IStarvationNutritionModifier
{
    public bool Enable { get; set; } = true;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(-0.05f)]
    public float Fruit { get; set; } = -0.05f;

    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(-0.075f)]
    public float Grain { get; set; } = -0.075f;

    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(-0.075f)]
    public float Vegetable{ get; set; } = -0.075f;

    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(-0.05f)]
    public float Protein { get; set; } = -0.05f;

    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(-0.1f)]
    public float Dairy { get; set; } = -0.1f;

    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.1f)]
    public float Base { get; set; } = 0.1f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.5f, 0.5f), DefaultValue(-0.2f)]
    public float Starvation { get; set; } = -0.25f;
}

public class WalkSpeedSettings : IStarvationNutritionModifier
{
    public bool Enable { get; set; } = true;
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.1f)]
    public float Fruit { get; set; } = 0.1f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.05f)]
    public float Grain { get; set; } = 0.05f;

    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.025f)]
    public float Vegetable { get; set; } = 0.025f;

    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.05f)]
    public float Protein { get; set; } = 0.05f;

    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.025f)]
    public float Dairy { get; set; } = 0.025f;

    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(-0.1f)]
    public float Base { get; set; } = -0.1f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.5f, 0.5f), DefaultValue(-0.2f)]
    public float Starvation { get; set; } = -0.2f;
}

public class MiningSpeedSettings : IStarvationNutritionModifier
{
    public bool Enable { get; set; } = true;
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.05f)]
    public float Fruit { get; set; } = 0.05f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.15f)]
    public float Grain { get; set; } = 0.15f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.1f)]
    public float Vegetable { get; set; } = 0.1f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.1f)]
    public float Protein { get; set; } = 0.1f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.05f)]
    public float Dairy { get; set; } = 0.05f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.15f)]
    public float Base { get; set; } = -0.15f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.5f, 0.5f), DefaultValue(-0.2f)]
    public float Starvation { get; set; } = -0.33f;
}

public class HealthSettings : IStarvationNutritionModifier
{
    [Description("Enables Overriding Vanilla health from nutrition bonus with the custom values below")]
    public bool Enable { get; set; } = true;
    [Range(0f, 10.0f), DefaultValue(3.5f)] 
    public float Fruit { get; set; } = 3.5f;

    [Range(0f, 10.0f), DefaultValue(2f)] 
    public float Grain{ get; set; } = 2f;

    [Range(0f, 10.0f), DefaultValue(4f)] 
    public float Vegetable{ get; set; } = 4f;

    [Range(0f, 10.0f), DefaultValue(2f)] 
    public float Protein { get; set; } = 2f;

    [Range(0f, 10.0f), DefaultValue(3.5f)]
    public float Dairy { get; set; } = 3.5f;

    [Range(-5f, 10.0f), DefaultValue(0f)]
    public float Base { get; set; } = 0f;

    [Range(-20f, 5f), DefaultValue(-7.5f)]
    public float Starvation { get; set; } = -7.5f;
}

public class MeleeDamageSettings : IStarvationNutritionModifier
{
    public bool Enable { get; set; } = true;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.075f)]
    public float Fruit { get; set; } = 0.075f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.05f)]
    public float Grain { get; set; } = 0.05f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.05f)]
    public float Vegetable { get; set; } = 0.05f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.15f)]
    public float Protein { get; set; } = 0.15f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(0.075f)]
    public float Dairy{ get; set; } = 0.075f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.2f, 0.2f), DefaultValue(-0.15f)]
    public float Base { get; set; } = -0.15f;
    
    [DisplayFormat(DataFormatString = "P"), Range(-0.5f, 0.5f), DefaultValue(-0.2f)]
    public float Starvation { get; set; } = -0.3f;
}