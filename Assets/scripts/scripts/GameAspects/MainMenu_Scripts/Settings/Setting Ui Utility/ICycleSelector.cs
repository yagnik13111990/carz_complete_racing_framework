using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICycleSelector<T> 
{
    int Count { get; }
    int IndexOf(T item);
    T Get(int index);

   

}
