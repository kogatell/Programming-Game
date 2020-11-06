using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Error : Object
{
    public const string Name = "Function";
    private string message;
    
    public Error(string msg)
    {
        message = msg;
    }

    public override string GetType()
    {
        return "Error";
    }

}
