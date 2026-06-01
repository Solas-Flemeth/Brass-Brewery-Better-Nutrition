namespace BetterNutrition;

public interface IStarvationNutritionModifier : INutritionModifier
{
   public float Starvation { get; }  
   public float Base { get; }
}