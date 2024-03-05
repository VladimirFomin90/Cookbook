

var cookbookApp = new CookbookApp(
    new RecipeRepository(),
    new RecipeConsoleUserInput());

cookbookApp.Run();

public class CookbookApp
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRecipeUserInput _recipeUserInput;

    public CookbookApp(
        IRecipeRepository recipeRepository,
        IRecipeUserInput recipeUserInput)
    {
        _recipeRepository = recipeRepository;
        _recipeUserInput = recipeUserInput;
    }

    public void Run()
    {
        var allRecipe = _recipeRepository.Read(filePath);
        _recipeUserInput.PrintExistRecipe(allRecipe);

        _recipeUserInput.CreateRecipe();

        var ingredient = _recipeUserInput.ReadIngredientFromUser();

        if (ingredient.Count > 0)
        {
            var recipe = new Recipe(ingredient);
            allRecipe.AddRecipe(recipe);
            _recipeRepository.Save(filePath, allRecipe);

            _recipeUserInput.ShowMessage("Рецепт добавлен:");
            _recipeUserInput.ShowMessage(recipe.ToString());
        }
        else
        {
            _recipeUserInput.ShowMessage("Не были выбраны ингредиенты");
        }

        _recipeUserInput.Exit();
    }
}

public interface IRecipeUserInput
{
    void ShowMessage(string message);
    void Exit();
}

public class RecipeConsoleUserInput : IRecipeUserInput
{
    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }
    public void Exit()
    {
        Console.WriteLine("Нажмите любую кнопку для закрытия");
        Console.ReadLine();
    }

}

public interface IRecipeRepository
{

}

public class RecipeRepository : IRecipeRepository
{
}