using Cookbook.Recipe;

var cookbookApp = new CookbookApp(
    new RecipeRepository(),
    new RecipeConsoleUserInput());

cookbookApp.Run("recipe.txt");

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

    public void Run(string filePath)
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

    void PrintExistRecipe(IEnumerable<Recipe> allRecipe)
    {
        if (allRecipe.Count() > 0)
        {
        }
    }
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

    public void PrintExistRecipe(IEnumerable<Recipe> allRecipe)
    {
        if (allRecipe.Count() > 0)
        {
            Console.WriteLine("Существуют рецепты:" + Environment.NewLine);

            for (int recipeIndex = 0; recipeIndex < allRecipe.Count(); recipeIndex++)
            {
                Console.WriteLine($"*****{recipeIndex + 1}*****");
                Console.WriteLine(allRecipe[recipeIndex]);
                Console.WriteLine();
            }
        }
    }
}

public interface IRecipeRepository
{
    List<Recipe> Read(string filePath);
}

public class RecipeRepository : IRecipeRepository
{
    public List<Recipe> Read(string filePath)
    {
        return new List<Recipe>
        {
            new(new List<Ingredient>
            {
                new Sugar(),
                new Flour()
            }),
            new(new List<Ingredient>(
            {
                new Cinnamon(),
                new Water()
            })
        };
    }
}