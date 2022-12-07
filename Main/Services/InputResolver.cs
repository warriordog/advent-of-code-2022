using System.Reflection;
using System.Text;
using AdventOfCode;

namespace Main.Services;

public interface IInputResolver
{
    public IEnumerable<IInputFile> GetRegisteredInputsForSolution(Type solutionType);
    public Task<string> LoadInputForSolution(SolutionEntry solution, IInputFile input);
    public Task<string> LoadInputByUserSelection(SolutionEntry solution, string? selectedInput, string? customInput);
}

public class InputResolver : IInputResolver
{

    public IEnumerable<IInputFile> GetRegisteredInputsForSolution(Type solutionType) => solutionType.GetCustomAttributes<InputFileAttribute>();


    public async Task<string> LoadInputByUserSelection(SolutionEntry solution, string? selectedInput, string? customInput)
    {
        // Custom input takes priority
        if (customInput != null) return await LoadInputFromCWD(customInput);
        
        // Otherwise, we load all registered inputs and work from there.
        var registeredInputs = GetRegisteredInputsForSolution(solution.SolutionType).ToList();
        if (!registeredInputs.Any()) throw new InvalidOperationException($"Cannot resolve input for solution [{solution.Day} {solution.Part} {solution.Variant}] because it has no registered services and a custom input was not provided");

        // Selected input takes next priority
        if (selectedInput != null) return await LoadSelectedInput(solution, registeredInputs, selectedInput);

        // Otherwise, we try to pick a default
        return await LoadDefaultInput(solution, registeredInputs);
    }
    
    private async Task<string> LoadSelectedInput(SolutionEntry solution, List<IInputFile> registeredInputs, string selectedInput)
    {
        var input = ResolveInputBySelection(registeredInputs, selectedInput);
        return await LoadInputForSolution(solution, input);
    }

    private async Task<string> LoadDefaultInput(SolutionEntry solution, List<IInputFile> registeredInputs)
    {
        var input = ResolveInputByDefaults(registeredInputs);
        return await LoadInputForSolution(solution, input);
    }

    public async Task<string> LoadInputForSolution(SolutionEntry solution, IInputFile input) => input.Resolution switch
    {
        InputFileResolution.EmbeddedResource => await LoadInputPathEmbeddedResource(solution, input.Path),
        InputFileResolution.EmbeddedResourceAbsolute => await LoadInputPathEmbeddedResourceAbsolute(input.Path),
        InputFileResolution.PathRelative => await LoadInputFromPathRelative(input.Path),
        InputFileResolution.PathAbsolute => await LoadInputFromPathAbsolute(input.Path),
#pragma warning disable CS0618
        InputFileResolution.PathRelativeToSolution => await LoadInputPathRelativeToSolution(solution, input.Path),
#pragma warning restore CS0618
        var other => throw new InvalidOperationException($"Unsupported InputFileResolution: {other}")
    };

    private async Task<string> LoadInputFromPathRelative(string inputPath) => await LoadInputFromCWD(inputPath);
    private async Task<string> LoadInputFromPathAbsolute(string inputPath) => await LoadInputFromCWD(inputPath);
    private async Task<string> LoadInputPathRelativeToSolution(SolutionEntry solution, string inputPath) => await LoadInputFromCWD($"AdventOfCode/{solution.Day}/{inputPath}");
    private async Task<string> LoadInputFromCWD(string inputPath) => await File.ReadAllTextAsync(inputPath);
    
    private async Task<string> LoadInputPathEmbeddedResourceAbsolute(string inputPath) => await LoadEmbeddedResource(inputPath);
    private async Task<string> LoadInputPathEmbeddedResource(SolutionEntry solution, string inputPath) => await LoadEmbeddedResource(inputPath, solution.SolutionType);
    private async Task<string> LoadEmbeddedResource(string inputPath, Type? scope = null)
    {
        await using var stream = OpenResourceStream(inputPath, scope);
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
    
    private Stream OpenResourceStream(string inputPath, Type? scope = null)
    {
        if (scope != null)
        {
            return scope.Assembly.GetManifestResourceStream(scope, inputPath)
                   ?? throw new FileNotFoundException($"Could not find embedded resource from scope {scope}", inputPath);
        }
        else
        {
            return typeof(ISolution).Assembly.GetManifestResourceStream(inputPath)
                   ?? throw new FileNotFoundException("Could not find unscoped embedded resource", inputPath);
        }
    }

    
    private static IInputFile ResolveInputBySelection(List<IInputFile> registeredInputs, string selectedInput)
    {
        // First, try to parse input as an indexed selector
        if (int.TryParse(selectedInput, out var index))
        {
            if (index < 0 || index >= registeredInputs.Count)
                throw new ArgumentException($"selectedInput is invalid - index is not in range [0-{registeredInputs.Count}] exclusive.", nameof(selectedInput));

            return registeredInputs[index];
        }

        // Then, try to match by name
        var inputByName = registeredInputs.Find(input => string.Equals(input.Name, selectedInput, StringComparison.OrdinalIgnoreCase));
        if (inputByName != null)
            return inputByName;

        // Next, try to match by path
        var inputByPath = registeredInputs.Find(input => string.Equals(input.Path, selectedInput, StringComparison.OrdinalIgnoreCase));
        if (inputByPath != null)
            return inputByPath;

        // Finally, try to match by type
        if (Enum.TryParse<InputFileType>(selectedInput, true, out var type))
        {
            var inputByType = registeredInputs
                .Where(input => input.Type == type)
                .MaxBy(input => input.IsDefault);
            if (inputByType != null)
            {
                return inputByType;
            }
        }

        // Didn't work, time to blow up
        throw new ArgumentException($"Input selection of \"{selectedInput}\" did not match any registered input.", nameof(selectedInput));
    }
    
    private static IInputFile ResolveInputByDefaults(List<IInputFile> registeredInputs) => registeredInputs
        .OrderByDescending(input => input.IsDefault)
        .ThenByDescending(input => input.Type == InputFileType.Standard)
        .First();
}