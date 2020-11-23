using System.Collections;
using System.Collections.Generic;
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
}
