namespace Cookbook.Recipe
{
    public class Recipe
    {
        public IEnumerable<Ingredient> Ingredients { get; }

        public Recipe(IEnumerable<Ingredient> ingredients)
        {
            Ingredients = ingredients;
        }
    }

    public abstract class Ingredient
    {
        public abstract int Id { get; }
        public abstract string Name { get; }
        public virtual string PrepareInstrucrion => "Добавь другие ингредиенты к репепту";
    }

    public class WheatFLour : Ingredient
    {
        public override int Id => 1;
        public override string Name => "Пшеничная мука";
        public override string PrepareInstrucrion => "Добавь другие ингредиенты";

    }
}
