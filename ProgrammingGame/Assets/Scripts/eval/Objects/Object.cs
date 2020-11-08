using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object
{
    public abstract string GetType();

    
    /// <summary>
    /// Returns if this object is an error
    /// </summary>
    /// <returns></returns>
    public bool IsError()
    {
        return GetType() == Error.Name;
    }

    public Object ToNumber()
    {
        if (IsError()) return this;
        if (IsNumeric()) return this;
        if (GetType() == Boolean.Name)
        {
            return ToBool() == Boolean.True ? new Number(1) : new Number(0);
        }
        if (GetType() != String.Name) return new Error($"can't convert object of type {GetType()} to numeric");
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
        if (GetType() == Null.Name)
        {
            return Boolean.False;
        }

        if (GetType() == Boolean.Name)
        {
            return this as Boolean;
        }
        
        return Boolean.True;
    }

    public bool IsNumeric()
    {
        return GetType() == Number.Name;
    }
} 