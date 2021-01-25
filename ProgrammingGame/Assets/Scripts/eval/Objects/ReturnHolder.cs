using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReturnHolder : Object
{
    private Object[] returns;

    public Object[] Returns => returns;
    
    public ReturnHolder(Object[] returns)
    {
        this.returns = returns;
    }
    
    public const string Name = "ReturnHolder";
    
    public override string Type()
    {
        return Name;
    }

    public override string ToString()
    {

        string result = "";
        foreach (Object returnHolder in returns)
        {
            if (result.Length > 0)
            {
                result += ", ";
            }
            result += returnHolder.ToString();
        }

        return result;
    }
}
