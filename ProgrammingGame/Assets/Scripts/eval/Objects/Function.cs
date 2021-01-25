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
    private FunctionDefinition definition;
    
    /// <summary>
    /// Keep a reference of the parent context every time.
    ///
    /// This is dangerous, we can have a stale context, so it's convenient to update the context of the function before calling it
    /// with <see cref="UpdateContext"/>
    /// 
    /// </summary>
    private Context parent;
    
    /// <summary>
    /// Keep track of all of the parents of this function instance, just in case for some reason there is a recursive call
    /// and that recursive call makes available to the parent its child variables.
    /// </summary>
    private Context[] parents = new Context[512];
    
    /// <summary>
    /// The current parent in the recursive call
    /// </summary>
    private int currentParent = -1;
    
    public Function(FunctionDefinition def, Context callerContext)
    {
        definition = def;
        parent = callerContext;
        parents[++currentParent] = callerContext;
    }

    public Object Call(Object[] objects)
    {
        Context ctx = new Context(parents[currentParent]);
        // Reset previous evaluator
        Eval localEvaluator = new Eval(ctx);
        if (currentParent >= 1) currentParent--;
        if (objects.Length != definition.ArgumentNames.Count)
        {
            return new Error($"error, Different number of arguments passed on a call of `{definition}`. Expected {objects.Length}, got: {definition.ArgumentNames.Count}");
        }
        // Debug.Log(definition);
        for (int i = 0; i < objects.Length; ++i)
        {
            Object obj = objects[i];
            ctx.Set(definition.ArgumentNames[i], obj.Clone());
        }
        
        return localEvaluator.EvaluateNode(definition.Block);
    }

    /// <summary>
    /// Previously we didn't have this function on the Function class. But we now really need it because
    /// we can't let user functions context become stale after calling them. This bug was discovered after
    /// using the same function over an over on a recursive call.
    /// </summary>
    /// <param name="parentCtx"></param>
    public void UpdateContext(Context parentCtx)
    {
        parents[++currentParent] = parentCtx;
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
