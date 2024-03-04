
var cookbookApp = new CookbookApp();

cookbookApp.Run();

public class CookbookApp
{
    private readonly RecipeRepository _recipeRepository;
    private readonly RecipeUserInput _recipeUserInput;

    public CookbookApp(RecipeRepository _recipeRepository, RecipeUserInput _recipeUserInput)
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

public class RecipeUserInput
{
}

public class RecipeRepository
{
}