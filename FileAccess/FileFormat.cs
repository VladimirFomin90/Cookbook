namespace Cookbook.FileAccess;

public class FileMetadata
{
    public FileMetadata(string name, FileFormat format)
    {
        Name = name;
        Format = format;
    }

    public string Name { get; }
    public FileFormat Format { get; }

    public string ToPath()
    {
        return $"{Name}.{Format.AsFileExtension()}";
    }
}

public static class FileFormatExtension
{
    public static string AsFileExtension(this FileFormat fileFormat)
    {
        return fileFormat == FileFormat.Json ? "json" : "txt";
    }
}

public enum FileFormat
{
    Json,
    Txt
}