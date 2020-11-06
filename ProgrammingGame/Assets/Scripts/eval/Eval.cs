using Relua.AST;
using UnityEngine;

/// <summary>
/// Eval evaluates a Lua AST on the run
/// </summary>
public class Eval
{
    private Context context;
    
    public Eval(Context ctx = null)
    {
        context = ctx ?? new Context();
    }


    private Object EvalExpr(IExpression expr)
    {
        if (expr == null)
        {
            return null;
        }
        string typeName = expr.GetType().Name;
        // When you wanna see the process of evaluation 
        // Debug.Log(typeName);
        //
        switch (typeName)
        {
            case Expressions.NumberLiteral:
            {
                NumberLiteral number = expr as NumberLiteral;
                return new Number(number);
            }
            
            case Expressions.Variable:
            {
                Variable variable = expr as Variable;
                return context.Get(variable.Name);
            }
            
            case Expressions.FunctionDefinition:
            {
                FunctionDefinition func = expr as FunctionDefinition;
                return new Function(func, context);
            }
            
            default:
                Debug.LogWarning($"unknown expression: {typeName}");
                break;
        }

        return null;
    }

    private Object EvalStatement(IStatement statement)
    {
        if (statement == null)
        {
            return null;
        }
        string typeName = statement.GetType().Name;
        // When you wanna see the process of evaluation 
        // Debug.Log(typeName);
        //
        switch (typeName)
        {
            case Statements.Assignment:
            {
                Assignment assignment = statement as Assignment;
                for (int i = 0; i < assignment.Values.Count; i++)
                {
                    IExpression expr = assignment.Values[i];
                    IAssignable assignable = assignment.Targets[i];
                    Object result = EvalExpr(expr);
                    EvalAssignable(assignable, result);
                }
                break;
            }

            case Statements.FunctionCall:
            {
                FunctionCall fc = statement as FunctionCall;
                Object function = EvalExpr(fc.Function);
                Object[] parameters = new Object[fc.Arguments.Count];
                for (int i = 0; i < fc.Arguments.Count; ++i)
                {
                    parameters[i] = EvalExpr(fc.Arguments[i]);
                    if (parameters[i] == null || parameters[i].GetType() == Error.Name)
                    {
                        return parameters[i];
                    }
                }

                if (function.GetType() != Function.Name)
                {
                    return new Error($"function call is not a function");
                }

                Function fn = function as Function;
                return fn.Call(parameters);
            }
            
            
                
            default: 
                Debug.LogWarning($"unknown statement {typeName}");
                break;
        }

        return null;
    }
    
    private void EvalAssignable(IAssignable assignable, Object target)
    {
        if (assignable == null) return;
        
        switch (assignable.GetType().Name)
        {
            case Assignables.Variable:
            {
                Variable variable = assignable as Variable;
                context.Set(variable.Name, target);
                break;
            }
            
            default:
                Debug.LogWarning($"unknown assignable: {assignable.GetType().Name}");
                break;
        }
        
    }
    
    public Object EvaluateNode(Node node)
    {
        if (node == null)
        {
            return null;
        }
        string typeName = node.GetType().Name;
        // When you wanna see the process of evaluation 
        // Debug.Log(typeName);
        //
        switch (typeName)
        {
            case "Block":
            {
                Block block = node as Block;
                for (int i = 0; i < block.Statements.Count; i++)
                {
                    IStatement statement = block.Statements[i];
                    Object obj = EvalStatement(statement);
                    if (i == block.Statements.Count - 1 && obj != null)
                    {
                        return obj;
                    }
                }
                break;
            }
                
            default:
                Debug.LogWarning($"unimplemented AST.Node: {typeName}");
                break;
        }
        
        return null;
    }
}
