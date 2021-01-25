using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boolean : Object
{
    private bool value = false;
    public const string Name = "Boolean";
    public static readonly Boolean True  = new Boolean(true);
    public static readonly Boolean False = new Boolean(false);

    public bool Value => value;
    
    private Boolean(bool b)
    {
        value = b;
    }

    public static Boolean FromBool(bool b)
    {
        return b ? True : False;
    }
    
    public override string Type()
    {
        return "Boolean";
    }

    public Boolean Bang()
    {
        return True == this ? False : True;
    }

    public override string ToString()
    {
        return value.ToString();
    }

    public static Boolean FromObject(Object boolean)
    {
        if (boolean is Boolean b) return b; 
        
        if (boolean is Number n)
        {
            return Boolean.FromBool(n.value != 0.0);
        }

        if (boolean is String str)
        {
            return Boolean.FromBool(str.Value.Length != 0);
        }

        return Boolean.FromBool(boolean != Null.NULL);
    }


    public override bool EqualDeep(Object target)
    {
        return this == target;
    }
}
