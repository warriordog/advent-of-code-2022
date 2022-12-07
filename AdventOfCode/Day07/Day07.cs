using System.Text;
using AdventOfCode.Common;

namespace AdventOfCode.Day07;

public abstract class Day07 : ISolution
{
    public void Run(string inputFile)
    {
        var history = inputFile
            .AsSpan()
            .TrimEnd()
            .SplitLines();

        var fileSystem = ParseFileSystem(history);
        RunDay7(fileSystem);
    }

    protected abstract void RunDay7(Directory rootDirectory);

    private Directory ParseFileSystem(SpanLineSplitter history)
    {
        var root = new Directory("/");
        var cwd = root;
        foreach (var line in history)
        {
            // Skip LS command because it doesn't matter.
            if (line.Equals("$ ls", StringComparison.Ordinal))
                continue;
            
            // Handle CD command
            if (line.StartsWith("$ cd ", StringComparison.Ordinal))
            {
                var dirName = line[5..].ToString();
                if (dirName.Equals("/", StringComparison.Ordinal))
                    cwd = root;
                else if (dirName.Equals("..", StringComparison.Ordinal))
                    cwd = cwd.Parent ?? throw new ApplicationException($"Attempting to CD out of the root directory");
                else
                    cwd = cwd.GetOrCreateDirectory(dirName);
                
                continue;
            }

            // Handle output.
            // Cheat - just treat everything else as a file or directory listing.
            if (line.StartsWith("dir ", StringComparison.Ordinal))
                cwd.GetOrCreateDirectory(line[4..].ToString());
            else 
                cwd.AddFile(ParseFile(line));
        }

        return root;
    }
    
    private FileEntry ParseFile(ReadOnlySpan<char> line)
    {
        var split = line.IndexOf(' ');
        var size = ulong.Parse(line[..split]);
        var name = line[(split + 1)..].ToString();
        return new FileEntry(name, size);
    }
}

public record Directory(string Name, Directory? Parent, Dictionary<string, Directory> Directories, Dictionary<string, FileEntry> Files)
{
    public Directory(string name, Directory? parent = null) : this(name, parent, new(), new()) {}

    public bool IsRoot => Parent == null;

    public string FullName => IsRoot ? "" : $"{Parent?.FullName}/{Name}";
    
    public IEnumerable<Directory> AllDirectories => Directories
        .Values
        .Concat(Directories
            .Values
            .SelectMany(d => d.AllDirectories)
        );

    public Directory GetOrCreateDirectory(string name)
    {
        if (!Directories.TryGetValue(name, out var dir))
        {
            dir = new Directory(name, this);
            Directories[name] = dir;
        }

        return dir;
    }

    public void AddFile(FileEntry file)
    {
        if (Files.ContainsKey(file.Name))
            throw new ArgumentException($"Cannot add duplicate file \"{file.Name}\"", nameof(file));

        Files[file.Name] = file;
    }

    // Mmm, yes we do like that LINQ
    public ulong GetTotalSize() =>
        Files.Values
            .Select(f => f.Size)
            .Aggregate(0ul, (sum, size) => sum + size)
        +
        Directories.Values
            .Select(d => d.GetTotalSize())
            .Aggregate(0ul, (sum, size) => sum + size);

    public override string ToString() => $"{Name} (dir)";

    public string DumpFilesystem()
    {
        var sb = new StringBuilder();
        DumpFilesystem(sb, "");
        return sb.ToString();
    }
    
    private void DumpFilesystem(StringBuilder builder, string indentation)
    {
        builder.Append(indentation);
        builder.Append("- ");
        builder.Append(ToString());
        builder.Append('\n');

        var childIndentation = indentation + "    ";
        foreach (var dir in Directories.Values)
        {
            dir.DumpFilesystem(builder, childIndentation);
        }
        foreach (var file in Files.Values)
        {
            builder.Append(childIndentation);
            builder.Append("- ");
            builder.Append(file);
            builder.Append('\n');
        }
    }
}

public record FileEntry(string Name, ulong Size)
{
    public override string ToString() => $"{Name} (file, size={Size})";
}