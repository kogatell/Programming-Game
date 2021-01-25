using System.Collections;
using System.Collections.Generic;
using Relua.AST;
using UnityEngine;

public interface Caller
{
    Object Call(Object[] parameters);
}

public class Function : Object, Caller
{
    public const string Name = "Function";
    
    private Eval localEvaluator;
    private FunctionDefinition definition;
    private Context ctx;
    
    public Function(FunctionDefinition def, Context callerContext)
    {
        definition = def;
        ctx = callerContext;
    }

    public Object Call(Object[] objects)
    {
        // Reset previous evaluator
        localEvaluator = new Eval(ctx);
        if (objects.Length != definition.ArgumentNames.Count)
        {
            return new Error($"error, Different number of arguments passed on a call of `{definition}`. Expected {objects.Length}, got: {definition.ArgumentNames.Count}");
        }
        // Debug.Log(definition);
        for (int i = 0; i < objects.Length; ++i)
        {
            Object obj = objects[i];
            ctx.Set(definition.ArgumentNames[i], obj);
        }
        return localEvaluator.EvaluateNode(definition.Block);
    }

    public override string Type()
    {
        return "Function";
    }

    public override string ToString()
    {
        return definition.ToString();
    }
}
