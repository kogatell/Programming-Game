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