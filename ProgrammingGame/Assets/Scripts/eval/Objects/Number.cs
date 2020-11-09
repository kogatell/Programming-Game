using System;
using System.Collections;
using System.Collections.Generic;
using Relua.AST;
using UnityEngine;

public class Number : Object, IEquatable<Number>, Hashable
{
    public const string Name = "Number";
    public double value;

    public double Value => value;

    public Number(NumberLiteral n)
    {
        value = n.Value;
    }
    
    public Number(double n)
    {
        value = n;
    }
    
    public override string Type()
    {
        return "Number";
    }

    public override string ToString()
    {
        return $"{value}";
    }

    public bool Equals(Number other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return value.Equals(other.value);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Number) obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
