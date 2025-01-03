using System;
using System.Text;
using UnityEngine;

public static class Utils
{
    public static Direction Opposite(this Direction dir)
    {
        return dir switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    public static void Print<T>(this System.Collections.Generic.ICollection<T> container)
    {
        var enumerator = container.GetEnumerator();
        
        StringBuilder stringBuilder = new();
        for (int i = 0; i < container.Count; i++)
        {
            enumerator.MoveNext();
            stringBuilder.Append($"{enumerator.Current}");
            if (i < container.Count - 1)
                stringBuilder.Append(", ");
        }

        Debug.Log(stringBuilder.ToString());
    }
}