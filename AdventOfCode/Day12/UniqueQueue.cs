namespace AdventOfCode.Day12;

/// <summary>
/// FIFO queue that does not allow duplicate entries.
/// </summary>
public class UniqueQueue<TItem>
{
    private readonly Queue<TItem> _queue = new();
    private readonly HashSet<TItem> _hashSet = new();

    public int Count => _queue.Count;
    
    public bool Enqueue(TItem item)
    {
        // Don't add duplicates
        if (!_hashSet.Add(item))
        {
            return false;
        }
        
        _queue.Enqueue(item);
        return true;
    }

    public TItem Dequeue()
    {
        var item = _queue.Dequeue();
        _hashSet.Remove(item);
        return item;
    }
}