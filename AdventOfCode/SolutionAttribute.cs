using JetBrains.Annotations;

namespace AdventOfCode;

/// <summary>
/// Indicates that a target class is an executable solution.
/// Target classes must implement <see cref="ISolution"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[MeansImplicitUse(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature, ImplicitUseTargetFlags.Itself | ImplicitUseTargetFlags.WithInheritors)]
public sealed class SolutionAttribute : Attribute
{
    /// <summary>
    /// Day of this solution.
    /// </summary>
    public string Day { get; }
    
    /// <summary>
    /// Part of this solution's day.
    /// Must be unique within the day.
    /// </summary>
    public string Part { get; }
    
    /// <summary>
    /// Optional variant of this solution's part.
    /// If not null, then must be unique within the day and part.
    /// </summary>
    public string? Variant { get; }

    public SolutionAttribute(string day, string part, string? variant = null)
    {
        Day = day;
        Part = part;
        Variant = variant;
    }
}