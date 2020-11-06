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
        
        switch (typeName)
        {
            case Expressions.FunctionDefinition:
            {
                FunctionDefinition func = expr as FunctionDefinition;
                Debug.Log(expr.ToString());
                return new Function(func, null);
            }
            
            default:
                Debug.LogWarning($"unknown expression: {typeName}");
                break;
        }

        return null;
    }

    private void EvalStatement(IStatement statement)
    {
        if (statement == null)
        {
            return;
        }
        string typeName = statement.GetType().Name;
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
                
            default: 
                Debug.LogWarning($"unknown statement {typeName}");
                break;
        }
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
    
    public Node EvaluateNode(Node node)
    {
        if (node == null)
        {
            return null;
        }
        Debug.Log($"Root {node.GetType().Name}");
        string typeName = node.GetType().Name;
        switch (typeName)
        {
            case "Block":
            {
                Block block = node as Block;
                foreach (IStatement statement in block.Statements)
                {
                    EvalStatement(statement);
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
