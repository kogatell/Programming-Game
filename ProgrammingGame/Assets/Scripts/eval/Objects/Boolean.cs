using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boolean : Object
{
    public bool value = false;

    public const string Name = "Boolean";
    public static readonly Boolean True  = new Boolean(true);
    public static readonly Boolean False = new Boolean(false);
    
    private Boolean(bool b)
    {
        value = b;
    }

    public static Boolean FromBool(bool b)
    {
        return b ? True : False;
    }
    
    public override string GetType()
    {
        return "Boolean";
    }

    public Boolean Bang()
    {
        return True == this ? False : True;
    }
    
    
}
