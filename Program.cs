using System.Text.Json;
using System.Text.Json.Nodes;
using Cookbook.Recipe;

const FileFormat Format = FileFormat.Json;

IStringRepository stringRepository =
    Format == FileFormat.Json ? new StringJsonRepository() : new StringTextualRepository();

const string FileName = "recipes";
var fileMetadata = new FileMetadata(FileName, Format);

var cookbookApp = new CookbookApp(
    new RecipeRepository(stringRepository, new IngredientRegister()),
    new RecipeConsoleUserInput(new IngredientRegister()));

cookbookApp.Run(fileMetadata.ToPath());

public class FileMetadata
{
    public FileMetadata(string name, FileFormat format)
    {
        Name = name;
        Format = format;
    }

    public string Name { get; }
    public FileFormat Format { get; }

    public string ToPath() => $"{Name}.{Format.AsFileExtension()}";
}

public static class FileFormatExtension
{
    public static string AsFileExtension(this FileFormat fileFormat) =>
        fileFormat == FileFormat.Json ? "json" : "txt";
}

public enum FileFormat
{
    Json,
    Txt
}

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
            _recipeRepository.Write(filePath, allRecipe);

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

public interface IIngredientRegister
{
    IEnumerable<Ingredient> All { get; }
    Ingredient GetById(int id);
}

public class IngredientRegister : IIngredientRegister
{
    public IEnumerable<Ingredient> All { get; } = new List<Ingredient>
    {
        new Chocolate(),
        new Cinnamon(),
        new Dough(),
        new Flour(),
        new Sugar(),
        new Water()
    };

    public Ingredient GetById(int id)
    {
        foreach (var ingredient in All)
            if (ingredient.Id == id)
                return ingredient;

        return null;
    }
}

public class RecipeConsoleUserInput : IRecipeUserInput
{
    private readonly IIngredientRegister _ingredientRegister;

    public RecipeConsoleUserInput(IIngredientRegister ingredientRegister)
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
        Console.WriteLine("Создание нового рецепта. " +
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

            if (int.TryParse(userInput, out var id))
            {
                var selectedIngredient = _ingredientRegister.GetById(id);

                if (selectedIngredient is not null) ingredient.Add(selectedIngredient);
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
    void Write(string filePath, List<Recipe> allRecipe);
}

public class RecipeRepository : IRecipeRepository
{
    private readonly IStringRepository _iStringRepository;
    private readonly IIngredientRegister _ingredientRegister;

    public RecipeRepository(IStringRepository iStringRepository, IIngredientRegister ingredientRegister)
    {
        _iStringRepository = iStringRepository;
        _ingredientRegister = ingredientRegister;
    }

    public List<Recipe> Read(string filePath)
    {
        var recipesFromFile = _iStringRepository.Read(filePath);
        var recipes = new List<Recipe>();

        foreach (var recipeFromFile in recipesFromFile)
        {
            var recipe = RecipeFromString(recipeFromFile);
            recipes.Add(recipe);
        }

        return recipes;
    }

    private Recipe RecipeFromString(string recipeFromFile)
    {
        var textIds = recipeFromFile.Split(",");
        var ingredients = new List<Ingredient>();

        foreach (var textId in textIds)
        {
            var id = int.Parse(textId);
            var ingredient = _ingredientRegister.GetById(id);
            ingredients.Add(ingredient);
        }

        return new Recipe(ingredients);
    }

    public void Write(string filePath, List<Recipe> allRecipe)
    {
        var recipeAsString = new List<string>();

        foreach (var recipe in allRecipe)
        {
            var allId = new List<int>();

            foreach (var ingredient in recipe.Ingredients) allId.Add(ingredient.Id);

            recipeAsString.Add(string.Join(",", allId));
        }

        _iStringRepository.Write(filePath, recipeAsString);
    }
}

public interface IStringRepository
{
    List<string> Read(string filePath);
    void Write(string filePath, List<string> strings);
}

public class StringTextualRepository : IStringRepository
{
    private static readonly string Separator = Environment.NewLine;

    public List<string> Read(string filePath)
    {
        if (File.Exists(filePath))
        {
            var fileContent = File.ReadAllText(filePath);
            return fileContent.Split(Separator).ToList();
        }

        return new List<string>();
    }

    public void Write(string filePath, List<string> strings)
    {
        File.WriteAllText(filePath, string.Join(Separator, strings));
    }
}

public class StringJsonRepository : IStringRepository
{
    public List<string> Read(string filePath)
    {
        if (File.Exists(filePath))
        {
            var fileContent = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<string>>(fileContent);
        }

        return new List<string>();
    }

    public void Write(string filePath, List<string> strings)
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(strings));
    }
}