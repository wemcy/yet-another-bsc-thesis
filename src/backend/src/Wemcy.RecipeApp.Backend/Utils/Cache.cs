using System.Diagnostics.CodeAnalysis;

namespace Wemcy.RecipeApp.Backend.Utils;

public class Cache<TKey, TValue>(int capacity) 
    where TKey : notnull
    where TValue: notnull
{
    private readonly object _lock = new();
    private readonly Dictionary<TKey, TValue> _dict = new(capacity);
    private readonly Queue<TKey> _keys = new(capacity);
    private readonly int _capacity = capacity;

    public void Add(TKey key, TValue value)
    {
        lock (_lock)
        {
            if (_dict.Count == _capacity)
            {
                var oldestKey = _keys.Dequeue();
                _dict.Remove(oldestKey);
            }

            _dict.Add(key, value);
            _keys.Enqueue(key);
        }
    }

    public bool TryGet(TKey key,[NotNullWhen(true)] out TValue? value)
    {
        lock (_lock)
        {
            return _dict.TryGetValue(key, out value);
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _keys.Clear();
            _dict.Clear();
        }
    }
}
