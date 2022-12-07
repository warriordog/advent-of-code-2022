using System.Reflection;
using AdventOfCode;
using Microsoft.Extensions.Logging;

namespace Main.Services;

public interface ISolutionRegistry
{
    public IEnumerable<IDayEntry> AllDays { get; }
    public IEnumerable<IDayEntry> GetDaysByFilter(string? day);

    public IEnumerable<SolutionEntry> AllSolutions { get; }
    public IEnumerable<SolutionEntry> GetSolutionsByFilter(string? day, string? part, string? variant);

    public void AddSolution(Type solutionType, string day, string part, string? variant = null);
    public void AddSolutions(Assembly assembly);
}

public class SolutionRegistry : ISolutionRegistry
{
    public IEnumerable<IDayEntry> AllDays => _days;
    private readonly List<DayEntry> _days = new();

    public IEnumerable<SolutionEntry> AllSolutions => _days
        .SelectMany(d => d.Parts
            .SelectMany(p => p.Solutions));

    private readonly ILogger<SolutionRegistry> _logger;
    public SolutionRegistry(ILogger<SolutionRegistry> logger) => _logger = logger;

    public IEnumerable<SolutionEntry> GetSolutionsByFilter(string? day, string? part, string? variant) => GetDaysByFilter(day)
        .SelectMany(d => d.GetPartsByFilter(part))
        .SelectMany(p => p.GetSolutionsByFilter(variant));

    public void AddSolution(Type solutionType, string dayName, string partName, string? variantName = null)
    {
        _logger.LogDebug("Adding solution for {day} {part} {variant}: {type}", dayName, partName, variantName, solutionType);
        
        // The variantName is equal to the partName for a solution that is not a variant
        variantName ??= partName;
        
        // Get or map the day
        var day = _days.Find(d => string.Equals(d.Name, dayName, StringComparison.OrdinalIgnoreCase));
        if (day == null)
        {
            day = new DayEntry(dayName);
            _days.Add(day);
            _days.Sort((p1, p2) => string.Compare(p1.Name, p2.Name, StringComparison.Ordinal));
        }

        // Get or map the part
        var part = day.GetOrCreatePart(partName);
        
        // Map the variant
        part.RegisterSolution(dayName, partName, variantName, solutionType);
    }
    
    public void AddSolutions(Assembly assembly)
    {
        _logger.LogDebug("Adding all solutions in assembly: {assembly}", assembly);
        foreach (var type in assembly.GetExportedTypes())
        {
            if (!type.IsAssignableTo(typeof(ISolution))) continue;
            
            var attribute = type.GetCustomAttribute<SolutionAttribute>();
            if (attribute == null) continue;
            
            AddSolution(type, attribute.Day, attribute.Part, attribute.Variant);
        }
    }
    
    public IEnumerable<IDayEntry> GetDaysByFilter(string? day)
    {
        if (day == null) return _days;
        return _days.Where(d => string.Equals(d.Name, day, StringComparison.OrdinalIgnoreCase));
    }
}

public interface IDayEntry
{
    public string Name { get; }
    public IEnumerable<IPartEntry> Parts { get; }

    public IEnumerable<IPartEntry> GetPartsByFilter(string? part);
}

internal class DayEntry : IDayEntry
{
    public string Name { get; }
    public IEnumerable<IPartEntry> Parts => _parts;
    private readonly List<PartEntry> _parts = new();
    
    public DayEntry(string name) => Name = name;

    public PartEntry GetOrCreatePart(string partName)
    {
        var part = _parts.Find(p => string.Equals(p.Name, partName, StringComparison.OrdinalIgnoreCase));
        if (part == null)
        {
            part = new PartEntry(partName);
            _parts.Add(part);
            _parts.Sort((p1, p2) => string.Compare(p1.Name, p2.Name, StringComparison.Ordinal));
        }

        return part;
    }

    public IEnumerable<IPartEntry> GetPartsByFilter(string? part)
    {
        if (part == null) return _parts;
        return _parts.Where(p => string.Equals(p.Name, part, StringComparison.OrdinalIgnoreCase));
    }
}

public interface IPartEntry
{
    public string Name { get; }
    public IEnumerable<SolutionEntry> Solutions { get; }
    public IEnumerable<SolutionEntry> GetSolutionsByFilter(string? variant);
}

internal class PartEntry : IPartEntry
{
    public string Name { get; }
    public IEnumerable<SolutionEntry> Solutions => _solutions;
    private readonly List<SolutionEntry> _solutions = new();

    public PartEntry(string name)
    {
        Name = name;
    }

    public void RegisterSolution(string dayName, string partName, string variantName, Type solutionType)
    {
        var entry = _solutions.Find(v => string.Equals(v.Variant, variantName, StringComparison.OrdinalIgnoreCase));
        
        if (entry != null)
        {
            // If already mapped, then we can just return.
            if (entry.SolutionType == solutionType)
                return;

            // If already mapped to a different type, then throw an exception
            throw new InvalidOperationException($"Attempting to re-assign {dayName} {partName} {variantName} from {entry.SolutionType} to {solutionType}");
        }
        
        // Not mapped, so add it
        entry = new SolutionEntry(solutionType, dayName, partName, variantName);
        _solutions.Add(entry);
        _solutions.Sort((p1, p2) => string.Compare(p1.Variant, p2.Variant, StringComparison.Ordinal));
    }

    public IEnumerable<SolutionEntry> GetSolutionsByFilter(string? variant)
    {
        if (variant == null) return Solutions;
        return Solutions.Where(s => string.Equals(s.Variant, variant, StringComparison.OrdinalIgnoreCase));
    }
}

public class SolutionEntry
{
    public bool IsVariant => Variant != Part;
    public Type SolutionType { get; }
    public string Day { get; }
    public string Part { get; }
    public string Variant { get; }
    
    public SolutionEntry(Type solutionType, string day, string part, string variant)
    {
        SolutionType = solutionType;
        Day = day;
        Part = part;
        Variant = variant;
    }
}