using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : Object
{
    public const string Name = "Slice";
    
    private int start;
    private int end;

    public Slice(Number left, Number right)
    {
        start = (int) left.value;
        end   = (int) right.value;
    }

    public int End => end;
    public int Start => start;

    public override string GetType()
    {
        return Name;
    }

    public override string ToString()
    {
        return $"{start}..{end}";
    }
}
