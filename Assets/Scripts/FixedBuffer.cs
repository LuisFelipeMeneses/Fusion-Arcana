using System.Collections.Generic;
using UnityEngine;

public class FixedBuffer<T>
{
    private T[] buffer;
    private int count = 0;

    public FixedBuffer(int size)
    {
        buffer = new T[size];
        count = 0;
    }

    public FixedBuffer(int size, T[] initialValues)
    {
        buffer = new T[size];
        count = Mathf.Min(initialValues.Length, size);

        for (int i = 0; i < count; i++)
        {
            buffer[i] = initialValues[i];
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

    public void Erase()
    {
        for (int i = 0; i < count; i++)
        {
            buffer[i] = default;
        }
        count = 0;
    }

    public T this[int index]
    {
        get => buffer[index];
        set => buffer[index] = value;
    }

    public override bool Equals(object obj)
    {
        if (obj is FixedBuffer<T> other && Count == other.Count)
        {
            for (int i = 0; i < Count; i++)
                if (!EqualityComparer<T>.Default.Equals(buffer[i], other.buffer[i]))
                    return false;
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        for (int i = 0; i < count; i++)
        {
            hash = hash * 31 + (buffer[i]?.GetHashCode() ?? 0);
        }
        return hash;
    }

    public int Count => count;
    public int Capacity => buffer.Length;

    public T[] ToArray()
    {
        T[] result = new T[buffer.Length];
        for (int i = 0; i < buffer.Length; i++)
            result[i] = buffer[i];
        return result;
    }

    public override string ToString()
    {
        string result = string.Empty;
        for (int i = 0; i < buffer.Length; i++)
            if (buffer[i] != null)
            {
                result += buffer[i].ToString() + ", ";
            } else
            {
                result += "[VAZIO], ";
            }
                return result;
    }
}