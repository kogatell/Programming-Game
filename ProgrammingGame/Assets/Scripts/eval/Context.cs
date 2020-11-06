using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context
{
    private Dictionary<string, Object> ctx;
    private Context parentCtx;

    public Context(Context parent = null)
    {
        ctx = new Dictionary<string, Object>();
        parentCtx = parent;
    }

    public Object Get(string varName)
    {
        
        if (!ctx.ContainsKey(varName))
        {
            if (parentCtx == null)
            {
                return new Error($"unknown variable: {varName}");
            }
            return parentCtx.Get(varName);
        }
        
        return ctx[varName];
    }

    public void Set(string varName, Object target)
    {
        ctx[varName] = target;
    }

}
