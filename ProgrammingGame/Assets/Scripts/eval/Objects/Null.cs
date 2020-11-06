using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Null : Object
{
    public const string Name = "Null";
    public static Null NULL = new Null();
    public override string GetType()
    {
        return "Null";
    }
}




