namespace AdventOfCode.Day12;

/// <summary>
/// Priority queue that does not allow duplicate entries.
/// </summary>
public class UniquePriorityQueue<TItem, TPriority>
{
    private readonly PriorityQueue<TItem, TPriority> _priorityQueue = new();
    private readonly HashSet<TItem> _hashSet = new();

    public int Count => _priorityQueue.Count;
    
    public bool Enqueue(TItem item, TPriority priority)
    {
        // Don't add duplicates
        if (!_hashSet.Add(item))
        {
            return false;
        }
        
        _priorityQueue.Enqueue(item, priority);
        return true;
    }

    public TItem Dequeue()
    {
        var item = _priorityQueue.Dequeue();
        _hashSet.Remove(item);
        return item;
    }
}