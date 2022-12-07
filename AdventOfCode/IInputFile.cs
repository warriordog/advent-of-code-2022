#pragma warning disable CS0618
namespace AdventOfCode;

/// <summary>
/// Defines an input file that can be loaded and passed to a solution.
/// </summary>
/// <remarks>
/// The relationship between input files and solutions is many-many.
/// Multiple inputs can be registered a single solution, and a single input can be registered to multiple solutions.
/// 
/// If multiple defaults are specified, then the one of them will be picked in an undefined manner.
/// If no defaults are specified, then any of type <see cref="InputFileType.Standard"/> will be picked in an undefined manner.
/// If no defaults are specified AND there are no standard inputs, then any available will be picked in an undefined manner.
/// If no inputs are specified and one cannot be selected from elsewhere (such as CLI arguments), then the solution will fail to run.
/// </remarks>
public interface IInputFile
{
    /// <summary>
    /// Path to the input file, relative to the working directory
    /// </summary>
    string Path { get; }

    /// <summary>
    /// Type of input file
    /// </summary>
    InputFileType Type { get; }

    /// <summary>
    /// Optional human-readable name for this input file.
    /// Will be shown to the user and can be used as an identifier to select this input.
    /// Does not need to be unique, but cannot be used for identification if there are duplicates.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Optional human-readable description of this input file.
    /// Should be 1-3 sentences in length.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Specifies how <see cref="Path"/> should be resolved.
    /// </summary>
    InputFileResolution Resolution { get; }
    
    /// <summary>
    /// If true, then this input should be prioritized as a default.
    /// </summary>
    bool IsDefault { get; }
}

/// <summary>
/// Determines how the input file should be located.
/// </summary>
public enum InputFileResolution
{
    /// <summary>
    /// Input file should be resolved as a filesystem path relative to the solution's source code directory.
    /// </summary>
    [Obsolete("This is unreliable and kept only for testing and rapid development")]
    PathRelativeToSolution,

    /// <summary>
    /// Input file should be resolved as a filesystem path relative to the current working directory.
    /// </summary>
    PathRelative,

    /// <summary>
    /// Input file should be resolved as an absolute filesystem path
    /// </summary>
    PathAbsolute,

    /// <summary>
    /// Input file should be resolved as an <a href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getmanifestresourcestream?view=net-7.0#system-reflection-assembly-getmanifestresourcestream(system-type-system-string)">embedded resource scoped to the solution class</a>.
    /// </summary>
    /// <remarks>
    /// To use, the file should be located in a directory structure matching the namespace of the solution.
    /// The filename should not include the path.
    /// Example: "input.txt"
    /// </remarks>
    EmbeddedResource,

    /// <summary>
    /// Input file should be resolved as an <a href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getmanifestresourcestream?view=net-7.0#system-reflection-assembly-getmanifestresourcestream(system-string)">unscoped embedded resource</a>.
    /// </summary>
    /// <remarks>
    /// To use, the filename should be the path to file separated by dots, followed by the filename
    /// Example: "AdventOfCode.Inputs.some_input.txt"
    /// </remarks>
    EmbeddedResourceAbsolute
}

public static class InputFileResolutionExtensions
{
    public static bool IsEmbedded(this InputFileResolution resolution) => resolution switch
    {
        InputFileResolution.EmbeddedResource => true,
        InputFileResolution.EmbeddedResourceAbsolute => true,
        _ => false
    };

    public static bool IsRelativeToCWD(this InputFileResolution resolution) => resolution switch
    {
        InputFileResolution.PathRelativeToSolution => true,
        InputFileResolution.PathRelative => true,
        _ => false
    };
    
    public static bool IsExternal(this InputFileResolution resolution) => resolution switch
    {
        InputFileResolution.PathRelativeToSolution => true,
        InputFileResolution.PathRelative => true,
        InputFileResolution.PathAbsolute => true,
        _ => false
    };
}

/// <summary>
/// Expected use case for an input file
/// </summary>
public enum InputFileType
{
    /// <summary>
    /// Standard input files from the AdventOfCode website.
    /// There should only be one of these per solution.
    /// </summary>
    Standard,
    
    /// <summary>
    /// Input files containing simpler or specific inputs for testing.
    /// </summary>
    Test,
    
    /// <summary>
    /// Challenge inputs to stress test a solution
    /// </summary>
    Challenge
}