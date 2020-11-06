using System.Collections;
using System.Collections.Generic;
using Relua.AST;
using UnityEngine;

public class Function : Object
{
    private Eval localEvaluator;
    private FunctionDefinition definition;
    
    public Function(FunctionDefinition def, Context callerContext)
    {
        localEvaluator = new Eval(callerContext);
        definition = def;
    }

    public Object Call(Object[] objects)
    {
        return null;
    }
}
