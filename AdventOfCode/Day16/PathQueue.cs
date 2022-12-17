using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Day16;

public class PathQueue
{
    public int BucketRange { get; init; } = 10;
    private readonly List<Queue<Path>> _buckets = new();

    public void Push(Path path)
    {
        var bucketKey = path.MinFlow / BucketRange;
        while (bucketKey >= _buckets.Count)
        {
            _buckets.Add(new Queue<Path>());
        }
        var bucket = _buckets[bucketKey];
        bucket.Enqueue(path);
    }

    public bool TryPop([NotNullWhen(true)] out Path? path)
    {
        for (var index = _buckets.Count - 1; index >= 0; index--)
        {
            var bucket = _buckets[index];
            if (bucket.TryDequeue(out path))
            {
                return true;
            }
        }

        path = null;
        return false;
    }
}