using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class String : Object
{
    private string value = "";
    
    public const string Name = "String";
    public string Value => value;

    public String(string value)
    {
        this.value = value;
    }
    
    public override string GetType()
    {
        return "String";
    }

    public override string ToString()
    {
        return $"\"{value}\"";
    }
    
}
