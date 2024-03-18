using Cookbook.Recipe;

var cookbookApp = new CookbookApp(
    new RecipeRepository(),
    new RecipeConsoleUserInput(new IngredientRegister()));

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

        if (ingredient.Count() > 0)
        {
            var recipe = new Recipe(ingredient);
            allRecipe.Add(recipe);
            // _recipeRepository.Write(filePath, allRecipe);

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

    void CreateRecipe();
    IEnumerable<Ingredient> ReadIngredientFromUser();
}

public class IngredientRegister
{
    public IEnumerable<Ingredient> All { get; } = new List<Ingredient>
    {
        new Flour(),
        new Sugar(),
        new Cinnamon(),
        new Dough(),
        new Chocolate(),
        new Water()
    };

    public Ingredient GetById(int id)
    {
        foreach (var ingredient in All)
        {
            if (ingredient.Id == id)
            {
                return ingredient;
            }
        }

        return null;
    }
}

public class RecipeConsoleUserInput : IRecipeUserInput
{
    private readonly IngredientRegister _ingredientRegister;

    public RecipeConsoleUserInput(IngredientRegister ingredientRegister)
    {
        _ingredientRegister = ingredientRegister;
    }

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

            var counter = 1;

            foreach (var recipe in allRecipe)
            {
                Console.WriteLine($"*****{counter}*****");
                Console.WriteLine(recipe);
                Console.WriteLine();
                ++counter;
            }
        }
    }

    public void CreateRecipe()
    {
        Console.WriteLine("Создание нового рецепта " +
                          "Доступные ингредиенты:");

        foreach (var ingredient in _ingredientRegister.All) Console.WriteLine(ingredient);
    }

    public IEnumerable<Ingredient> ReadIngredientFromUser()
    {
        var shallStop = false;
        var ingredient = new List<Ingredient>();

        while (!shallStop)
        {
            Console.WriteLine("Добавь ингридиент по номеру");

            var userInput = Console.ReadLine();

            if (int.TryParse(userInput, out int id))
            {
                var selectedIngredient = _ingredientRegister.GetById(id);

                if (selectedIngredient is not null)
                {
                    ingredient.Add(selectedIngredient);
                }
            }
            else
            {
                shallStop = true;
            }
        }

        return ingredient;
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
            new(new List<Ingredient>
            {
                new Cinnamon(),
                new Water()
            })
        };
    }
}