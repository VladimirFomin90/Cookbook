namespace Cookbook.Recipe;

public abstract class Ingredient
{
    public abstract int Id { get; }
    public abstract string Name { get; }
    public virtual string PrepareInstruction => "Добавь другие ингредиенты к репепту";
}