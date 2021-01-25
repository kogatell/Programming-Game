using System;
using UnityEngine;

public class MathLib : Lib
{
    
    public override Context InjectLibrary(Context context)
    {
        Table table = new Table();
        table.Set(new String("floor"), new StdLibFunc(Floor));
        table.Set(new String("min"), new StdLibFunc(Min));
        table.Set(new String("max"), new StdLibFunc(Max));
        table.Set(new String("abs"), new StdLibFunc(Abs));
	    context.Set("math", table);
        return context;
    }

    private Object Floor(Object[] parameters)
    {
        if (parameters.Length != 1)
        {
            return new Error($"expected 1 parameter on floor, got {parameters.Length}");
        }
        if (parameters[0] is Number n)
        {
            return new Number(Math.Floor(n.value));    
        }
        return new Error($"expected parameter to be a number, got: {parameters[0].Type()}");
    }

    private Object Abs(Object[] parameters)
    {
        if (parameters.Length != 1)
        {
            return new Error($"expected 1 parameter or more on abs, got {parameters.Length}");
        }

        if (parameters[0] is Number n)
        {
            return new Number(Math.Abs(n.value));
        }
        return new Error($"expected parameter on abs to be a number, got {parameters[0].Type()}");
    }


    private Object Min(Object[] parameters)
    {
        if (parameters.Length == 0)
        {
            return new Error($"expected 1 parameter or more on floor, got {parameters.Length}");
        }
        if (parameters[0] is Number)
        {
            double min = 0.0;
            foreach (Object param in parameters)
            {
                if (param is Number p)
                {
                    min = Math.Min(p.value, min);
                }
                else
                {
                    return new Error($"expected one of the parameters to be a number, got: {param.Type()}");
                }
            }
            return new Number(min);
        }
        if (parameters[0] is ArrayObject array)
        {
            var objects = array.array;
            double min = 0.0;
            foreach (Object param in objects)
            {
                if (param is Number p)
                {
                    min = Math.Min(p.value, min);
                }
                else
                {
                    return new Error($"expected one of the array elements to be a number, got: {param.Type()}");
                }
            }
            return new Number(min);
        }
        return new Error($"expected parameter to be a number or array of numbers, got: {parameters[0].Type()}");
    }
    
    private Object Max(Object[] parameters)
    {
        if (parameters.Length == 0)
        {
            return new Error($"expected 1 parameter or more on floor, got {parameters.Length}");
        }
        if (parameters[0] is Number)
        {
            double max = 0.0;
            foreach (Object param in parameters)
            {
                if (param is Number p)
                {
                    max = Math.Max(p.value, max);
                }
                else
                {
                    return new Error($"expected one of the parameters to be a number, got: {param.Type()}");
                }
            }
            return new Number(max);
        }
        if (parameters[0] is ArrayObject array)
        {
            var objects = array.array;
            double max = 0.0;
            if (objects.Count == 0) return new Error("expected array to have at least one number on max"); 
            foreach (Object param in objects)
            {
                if (param is Number p)
                {
                    max = Math.Max(p.value, max);
                }
                else
                {
                    return new Error($"expected one of the array elements to be a number, got: {param.Type()}");
                }
            }
            return new Number(max);
        }
        return new Error($"expected parameter to be a number or array of numbers, got: {parameters[0].Type()}");
    }
}
