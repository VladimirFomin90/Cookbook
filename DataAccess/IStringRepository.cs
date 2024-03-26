using System.Text.Json;

namespace Cookbook.DataAccess;

public interface IStringRepository
{
    List<string> Read(string filePath);
    void Write(string filePath, List<string> strings);
}

public abstract class StringRepository : IStringRepository
{
    public List<string> Read(string filePath)
    {
        if (File.Exists(filePath))
        {
            var fileContent = File.ReadAllText(filePath);
            return TextToString(fileContent);
        }

        return new List<string>();
    }

    protected abstract List<string> TextToString(string fileContent);

    public void Write(string filePath, List<string> strings)
    {
        File.WriteAllText(filePath, StringToText(strings));
    }

    protected abstract string StringToText(List<string> strings);
}

public class StringTextualRepository : StringRepository
{
    private static readonly string Separator = Environment.NewLine;

    protected override List<string> TextToString(string fileContent)
    {
        return fileContent.Split(Separator).ToList();
    }

    protected override string StringToText(List<string> strings)
    {
        return string.Join(Separator, strings);
    }
}

public class StringJsonRepository : StringRepository
{
    protected override List<string> TextToString(string fileContent)
    {
        return JsonSerializer.Deserialize<List<string>>(fileContent);
    }

    protected override string StringToText(List<string> strings)
    {
        return JsonSerializer.Serialize(strings);
    }

}