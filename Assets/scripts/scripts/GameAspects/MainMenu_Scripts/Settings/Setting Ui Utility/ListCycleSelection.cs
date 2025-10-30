using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListCycleSelection<T> : ICycleSelector<T>
{
    private readonly List<T> Content;

    public ListCycleSelection(List<T> content) => this.Content = content;
    public int Count => this.Content.Count;

    public T Get(int index)
    {
        return this.Content[index];
    }

    public int IndexOf(T item)
    {
       return this.Content.IndexOf(item);
    }
}
