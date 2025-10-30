using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EnumList<T> where T : Enum
{
    public static readonly List<T> Types = new List<T>((T[])Enum.GetValues(typeof(T)));

}
