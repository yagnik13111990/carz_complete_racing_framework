using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumCycleSelecton <T> : ICycleSelector<T> where T : Enum
{
    private readonly List<T> Content;

    public EnumCycleSelecton()
    {
        Content = new List<T>((T[]) Enum.GetValues(typeof(T)));
    }

    public int Count => Content.Count;

    public T Get(int index)
    {
       return Content[index];
    }

    public int IndexOf(T item)
    {
        return Content.IndexOf(item);
    }

   
}
