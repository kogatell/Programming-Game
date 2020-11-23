using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Error : Object
{
    public const string Name = "Error";
    private string message;

    public string Message => message;
    
    public Error(string msg)
    {
        message = msg;
    }

    public override string Type()
    {
        return "Error";
    }

    public override string ToString()
    {
        return message;
    }
}
