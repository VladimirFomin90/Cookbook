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
        var step = Ingredients
            .Select(ingredient =>
                $"{ingredient.Name}. {ingredient.PrepareInstruction}");

        return string.Join(Environment.NewLine, step);
    }
}