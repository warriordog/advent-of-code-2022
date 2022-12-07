namespace AdventOfCode;

/// <summary>
/// Registers an input file for use with a particular solution 
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class InputFileAttribute : Attribute, IInputFile
{
    public string Path { get; }
    public InputFileType Type { get; }
    public string? Name { get; }
    public string? Description { get; }
    public InputFileResolution Resolution { get; }
    public bool IsDefault { get; }

    public InputFileAttribute(string path, InputFileType type = InputFileType.Standard, string? name = null, string? description = null, InputFileResolution resolution = InputFileResolution.EmbeddedResource, bool isDefault = false)
    {
        Path = path;
        IsDefault = isDefault;
        Resolution = resolution;
        Type = type;
        Description = description;
        Name = name;
    }
}