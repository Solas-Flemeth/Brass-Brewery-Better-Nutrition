using System.ComponentModel;

namespace BetterNutrition;

public class IntegrationSettings
{
    [Description("Integration for the XSkills Mod")]
    public XSkillsSettings XSkills = new XSkillsSettings();
    
    [Description("Integration for the Hydrate or Diedrate Mod")]
    public HydrateOrDiedrateSettings HydrateOrDiedrate = new HydrateOrDiedrateSettings();
}