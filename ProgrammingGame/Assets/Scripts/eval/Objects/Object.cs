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
} 