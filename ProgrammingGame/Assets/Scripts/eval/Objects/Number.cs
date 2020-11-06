using System.Collections;
using System.Collections.Generic;
using Relua.AST;
using UnityEngine;

public class Number : Object
{
    public const string Name = "Number";
    public double value;

    public Number(NumberLiteral n)
    {
        value = n.Value;
    }
    
    public override string GetType()
    {
        return "Number";
    }
}
