using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6;
public interface IProbingStrategy<TKey>
{
    int Probe(int initialIndex, int step, int tableSize, TKey key);
}

// Линейное исследование
public class LinearProbing<TKey> : IProbingStrategy<TKey>
{
    public int Probe(int initialIndex, int step, int tableSize, TKey key)
    {
        return (initialIndex + step) % tableSize;
    }
}

// Квадратичное исследование
public class QuadraticProbing<TKey> : IProbingStrategy<TKey>
{
    public int Probe(int initialIndex, int step, int tableSize, TKey key)
    {
        return (initialIndex + step * step) % tableSize;
    }
}

// Двойное хеширование
public class DoubleHashing<TKey> : IProbingStrategy<TKey>
{
    public int Probe(int initialIndex, int step, int tableSize, TKey key)
    {
        int hashCode2 = 1 + Math.Abs(key.GetHashCode()) % (tableSize - 1);
        return (initialIndex + step * hashCode2) % tableSize;
    }
}

public class HashTableOpenAddressing<TKey, TValue>
{
    public int capacity; 
    private const float LoadFactorThreshold = 0.70f;
    private int count;
    private Entry[] entries;
    private IProbingStrategy<TKey> probingStrategy;


    private struct Entry
    {
        public TKey Key;
        public TValue Value;
        public bool IsDeleted;
    }

    public HashTableOpenAddressing(IProbingStrategy<TKey> probingStrategy)
    {
        capacity = 10000;
        entries = new Entry[10000];
        count = 0;
        this.probingStrategy = probingStrategy;

    }

    public int GetBucketIndex(TKey key, int capacity)
    {
        int hashCode = key.GetHashCode();
        return Math.Abs(hashCode) % capacity;
    }

    public void Add(TKey key, TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        if ((float)count / entries.Length >= LoadFactorThreshold)
        {
            Resize(2 * entries.Length);
        }

        int index = GetBucketIndex(key, entries.Length);
        int originalIndex = index;
        int step = 0;

        while (!entries[index].IsDeleted && entries[index].Key == null)
        {
            if (entries[index].Key.Equals(key))
            {
                entries[index].Value = value;
                return;
            }

            step++;
            index = probingStrategy.Probe(originalIndex, step, entries.Length, key);

        }

        entries[index].Key = key;
        entries[index].Value = value;
        entries[index].IsDeleted = false;
        count++;
    }

    public bool Remove(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int index = GetBucketIndex(key, entries.Length);
        int originalIndex = index;
        int step = 0; 

        while (entries[index].Key != null)
        {
            if (entries[index].Key.Equals(key) && !entries[index].IsDeleted)
            {
                entries[index].IsDeleted = true;
                count--;
                return true;
            }

            step++;
            index = probingStrategy.Probe(originalIndex, step, entries.Length, key);

            if (index == originalIndex)
            {
                break;
            }
        }

        return false;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int index = GetBucketIndex(key, entries.Length);
        int originalIndex = index;
        int step = 0; 

        while (entries[index].Key != null)
        {
            if (entries[index].Key.Equals(key) && !entries[index].IsDeleted)
            {
                value = entries[index].Value;
                return true;
            }

            step++;
            index = probingStrategy.Probe(originalIndex, step, entries.Length, key);

            if (index == originalIndex)
            {
                break;
            }
        }

        value = default(TValue);
        return false;
    }

    public bool ContainsKey(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int index = GetBucketIndex(key, entries.Length);
        int originalIndex = index;
        int step = 0;

        // Ищем элемент
        while (entries[index].Key != null)
        {
            if (entries[index].Key.Equals(key) && !entries[index].IsDeleted)
            {
                return true;
            }

            step++;
            index = probingStrategy.Probe(originalIndex, step, entries.Length, key);

            if (index == originalIndex)
            {
                break;
            }
        }

        return false;
    }

    public void Clear()
    {
        entries = new Entry[capacity];
        count = 0;
    }

    public float GetLoadFactor()
    {
        return (float)count / entries.Length;
    }

    private void Resize(int newCapacity)
    {
        var newEntries = new Entry[newCapacity];

        foreach (var entry in entries)
        {
            if (entry.Key != null && !entry.IsDeleted)
            {
                int index = GetBucketIndex(entry.Key, newCapacity);
                int step = 0;

                while (newEntries[index].Key == null)
                {
                    step++;
                    index = probingStrategy.Probe(index, step, newCapacity, entry.Key);
                }

                newEntries[index] = entry;
            }
        }

        entries = newEntries;
        capacity = newCapacity;
    }
    public int GetLongestClusterLength()
    {
        int maxClusterLength = 0;
        int currentClusterLength = 0;

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].Key != null && !entries[i].IsDeleted)
            {
                currentClusterLength++;
            }
            else
            {
                if (currentClusterLength > maxClusterLength)
                {
                    maxClusterLength = currentClusterLength;
                }
                currentClusterLength = 0; 
            }
        }

        if (currentClusterLength > maxClusterLength)
        {
            maxClusterLength = currentClusterLength;
        }

        return maxClusterLength;
    }
    public IEnumerable<KeyValuePair<TKey, TValue>> GetAllItems()
    {
        foreach (var entry in entries)
        {
            if (entry.Key != null && !entry.IsDeleted)
            {
                yield return new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
            }
        }
    }
}
