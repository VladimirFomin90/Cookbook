namespace Cookbook.Recipe;

public class Recipe
{
    public IEnumerable<Ingredient> Ingredients { get; }

    public Recipe(IEnumerable<Ingredient> ingredients)
    {
        Ingredients = ingredients;
    }

    public override string ToString()
    {
        var step = new List<string>();

        foreach (var ingredient in Ingredients) step.Add($"{ingredient.Name}. {ingredient.PrepareInstruction}");

        return string.Join(Environment.NewLine, step);
    }
}