namespace BetterNutrition;

public interface INutritionModifier
{ 
    public bool Enable { get; }
    public float Fruit { get; }
    public float Grain { get; }
    public float Vegetable { get; }
    public float Protein { get; }
    public float Dairy { get; }

}