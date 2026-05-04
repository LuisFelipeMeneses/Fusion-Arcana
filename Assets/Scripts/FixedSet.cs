using System;
using System.Collections.Generic;
using System.Linq;

public class FixedSet<T>
{
    private T[] buffer;
    private int count;

    public FixedSet(int size)
    {
        buffer = new T[size];
        count = 0;
    }

    public FixedSet(int size, IEnumerable<T> values)
    {
        buffer = new T[size];
        count = 0;

        foreach (var v in values)
        {
            Add(v);
        }
    }

    public void Add(T item)
    {
        if (count < buffer.Length)
        {
            buffer[count++] = item;
        }
        else
        {
            for (int i = 1; i < count; i++)
            {
                buffer[i - 1] = buffer[i];
            }

            buffer[count - 1] = item;
        }
    }

    public T Remove(int i)
    {
        if (i < 0 || i >= count)
            throw new System.ArgumentOutOfRangeException(nameof(i), "Index out of range!");
        T item = buffer[i];
        for (int j = i + 1; j < count; j++)
        {
            buffer[j - 1] = buffer[j];
        }
        buffer[count - 1] = default;
        count--;
        return item;
    }

    public T RemoveFirst()
    {
        if (count == 0)
            throw new System.InvalidOperationException("Empty Buffer!");

        T item = buffer[0];

        for (int i = 1; i < count; i++)
        {
            buffer[i - 1] = buffer[i];
        }

        buffer[count - 1] = default;
        count--;

        return item;
    }

    public bool Contains(T item)
    {
        var comparer = EqualityComparer<T>.Default;

        for (int i = 0; i < count; i++)
        {
            if (comparer.Equals(buffer[i], item))
                return true;
        }

        return false;
    }

    public int Count => count;
    public int Capacity => buffer.Length;

    public T[] ToArray(bool includeEmpty = false)
    {
        if (!includeEmpty)
        {
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
                result[i] = buffer[i];
            return result;
        }

        T[] full = new T[buffer.Length];
        for (int i = 0; i < buffer.Length; i++)
            full[i] = buffer[i];

        return full;
    }

    public override bool Equals(object obj)
    {
        if (obj is not FixedSet<T> other || other.count != count)
            return false;

        var comparer = EqualityComparer<T>.Default;

        for (int i = 0; i < count; i++)
        {
            if (!other.Contains(buffer[i]))
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        int hash = 0;

        for (int i = 0; i < count; i++)
        {
            hash ^= buffer[i]?.GetHashCode() ?? 0;
        }

        return hash;
    }

    public override string ToString()
    {
        return "[" + string.Join(", ",
            buffer.Take(count).Select(x => x?.ToString())) + "]";
    }
}