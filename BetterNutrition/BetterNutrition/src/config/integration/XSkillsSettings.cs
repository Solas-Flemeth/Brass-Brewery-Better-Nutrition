using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BetterNutrition;

public class XSkillsSettings
{
    public bool Enable { get; set; } = false;
    public XpMultiplier XSkillMultiplier { get; set; } = new XpMultiplier();
}
public class XpMultiplier : INutritionModifier
{
    [Description("Should the player gain additional XSkills XP for having good nutrition. This is great for having better XP gain late game. This requires a restart to work")]
    public bool Enable { get; set; } = true;
    
    [DisplayFormat(DataFormatString = "P"), Range(0f, 0.5f), DefaultValue(0.06f)]
    public float Fruit { get; set; } = 0.06f;
    
    [DisplayFormat(DataFormatString = "P"), Range(0f, 0.5f), DefaultValue(0.06f)]
    public float Grain { get; set; } = 0.06f;
    
    [DisplayFormat(DataFormatString = "P"), Range(0f, 0.5f), DefaultValue(0.06f)]
    public float Vegetable { get; set; } = 0.06f;
    
    [DisplayFormat(DataFormatString = "P"), Range(0f, 0.5f), DefaultValue(0.06f)]
    public float Protein { get; set; } = 0.06f;
    
    [DisplayFormat(DataFormatString = "P"), Range(0f, 0.5f), DefaultValue(0.06f)]
    public float Dairy { get; set; } = 0.06f;
}