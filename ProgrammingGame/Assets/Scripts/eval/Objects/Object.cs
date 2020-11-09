using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object
{
    public abstract string Type();
    
    /// <summary>
    /// Returns if this object is an error
    /// </summary>
    /// <returns></returns>
    public bool IsError()
    {
        return Type() == Error.Name;
    }

    public Object ToNumber()
    {
        if (IsError()) return this;
        if (IsNumeric()) return this;
        if (Type() == Boolean.Name)
        {
            return ToBool() == Boolean.True ? new Number(1) : new Number(0);
        }
        if (Type() != String.Name) return new Error($"can't convert object of type {Type()} to numeric");
        try
        {
            return new Number(int.Parse((this as String).Value));
        }
        catch (Exception)
        {
            return new Error($"error converting string {this} to number");
        }
    }

    public Boolean ToBool()
    {
        if (Type() == Null.Name)
        {
            return Boolean.False;
        }

        if (Type() == Boolean.Name)
        {
            return this as Boolean;
        }
        
        return Boolean.True;
    }

    public bool IsNumeric()
    {
        return Type() == Number.Name;
    }
} 