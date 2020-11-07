// ReSharper disable PossibleNullReferenceException
// ReSharper disable CompareOfFloatsByEqualityOperator

using System;
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
            return Null.NULL;
        }
        string typeName = expr.GetType().Name;
        // When you wanna see the process of evaluation 
        // Debug.Log(typeName);
        //
        switch (typeName)
        {
            // When a function call is called from an assignment or return
            case Statements.FunctionCall:
                return EvalStatement(expr as FunctionCall);

            case Statements.BinaryOp:
            {
                BinaryOp binOpExpr = expr as BinaryOp;
                Object objLeft  = EvalExpr(binOpExpr.Left);
                if (objLeft.IsError()) return objLeft;
                Object objRight = EvalExpr(binOpExpr.Right);
                if (objRight.IsError()) return objRight;
                return EvaluateBinaryOperation(objLeft, objRight, binOpExpr.Type);
            }
            
            case Expressions.NumberLiteral:
            {
                NumberLiteral number = expr as NumberLiteral;
                return new Number(number);
            }
            
            case Expressions.Variable:
            {
                Variable variable = expr as Variable;
                //Debug.Log(variable);
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

        return Null.NULL;
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
            case Statements.NumericFor:
            {
                NumericFor forStmt = statement as NumericFor;
                Object startPoint = EvalExpr(forStmt.StartPoint);
                if (startPoint.IsError()) return startPoint;
                Object endPoint = EvalExpr(forStmt.EndPoint);
                if (endPoint.IsError()) return endPoint;
                Object step = EvalExpr(forStmt.Step);
                if (step.IsError()) return step;
                return EvaluateNumericForLoop(startPoint, endPoint, step, forStmt.Block, forStmt.VariableName);
            }
            
            case Statements.Assignment:
            {
                Assignment assignment = statement as Assignment;
                int j, i;
                for (i = 0, j = 0; i < assignment.Values.Count; i++)
                {
                    IExpression expr = assignment.Values[i];
                    Object result = EvalExpr(expr);
                    if (result.IsError()) return result;
                    if (result.GetType() != ReturnHolder.Name)
                    {
                        if (j >= assignment.Targets.Count) return new Error("too few variables in destructuring");
                        IAssignable assignable = assignment.Targets[j];
                        EvalAssignable(assignable, result);
                        j++;
                        continue;
                    }
                    ReturnHolder returnHold = result as ReturnHolder;
                    foreach (Object returnVal in returnHold.Returns)
                    {
                        if (j >= assignment.Targets.Count) return new Error("too few variables in destructuring");
                        IAssignable assignable = assignment.Targets[j];
                        EvalAssignable(assignable, returnVal);
                        j++;
                    }
                }
                if (j != assignment.Targets.Count)
                {
                    return new Error("too much variables in destructuring");
                }
                break;
            }

            case Statements.If:
            {
                If ifStmt = statement as If;
                if (EvalExpr(ifStmt.MainIf.Condition).ToBool().value)
                {
                    return EvaluateNode(ifStmt.MainIf.Block);
                }
                
                if (ifStmt.ElseIfs != null)
                {
                    foreach (ConditionalBlock condition in ifStmt.ElseIfs)
                    {
                        if (EvalExpr(condition.Condition).ToBool().value)
                        {
                            return EvaluateNode(condition.Block);
                        }
                    }
                }
                
                if (ifStmt.Else != null)
                {
                    return EvaluateNode(ifStmt.Else);
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
                    if (parameters[i].GetType() == Error.Name)
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
            
            case Statements.Return:
            {
                Return returnStmt = statement as Return;
                // Don't return a return holder because evaluating a ReturnHolder on
                // every Exp parsing would be a mess.
                if (returnStmt.Expressions.Count == 1)
                {
                    return EvalExpr(returnStmt.Expressions[0]);
                }
                // Now we know there are 2 variables at least being returned by this function.
                // Create a return holder which will be parsed in the Assignment case
                Object[] returnsRes = new Object[returnStmt.Expressions.Count];
                for (int i = 0; i < returnsRes.Length; ++i)
                {
                    returnsRes[i] = EvalExpr(returnStmt.Expressions[i]);
                    if (returnsRes[i].IsError()) return returnsRes[i];
                }
                return new ReturnHolder(returnsRes);
            }

            default: 
                Debug.LogWarning($"unknown statement {typeName}");
                break;
        }

        return Null.NULL;
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
            return Null.NULL;
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
                    if (obj.IsError()) return obj;
                    if (i == block.Statements.Count - 1)
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
        
        return Null.NULL;
    }


    private Object EvaluateBinaryOperation(Object left, Object right, BinaryOp.OpType type)
    {
        if (left.GetType() != right.GetType())
        {
            return new Error($@"
                Unsupported different types in {type} operation
                left side is a: {left.GetType()}
                right side is a: {right.GetType()}
            ");
        }
        if (left.GetType() == Number.Name && right.GetType() == Number.Name)
        {
            return EvaluateNumberOp(left as Number, right as Number, type);
        }
        return Null.NULL;
    }

    private Object EvaluateNumberOp(Number left, Number right, BinaryOp.OpType type)
    {
        switch (type)
        {
            case BinaryOp.OpType.Add:
                return new Number(left.value + right.value);
            case BinaryOp.OpType.Subtract:
                return new Number(left.value - right.value);
            case BinaryOp.OpType.Divide:
                return new Number(left.value / right.value);
            case BinaryOp.OpType.Power:
                return new Number(Math.Pow(left.value, right.value));
            case BinaryOp.OpType.Multiply:
                return new Number(left.value * right.value);
            case BinaryOp.OpType.Modulo:
                return new Number(left.value % right.value);
            case BinaryOp.OpType.Equal:
                return Boolean.FromBool(left.value == right.value);
            case BinaryOp.OpType.GreaterThan:
                return Boolean.FromBool(left.value > right.value);
            case BinaryOp.OpType.GreaterOrEqual:
                return Boolean.FromBool(left.value >= right.value);
            case BinaryOp.OpType.LessThan:
                return Boolean.FromBool(left.value < right.value);
            case BinaryOp.OpType.LessOrEqual:
                return Boolean.FromBool(left.value <= right.value);
            case BinaryOp.OpType.NotEqual:
                return Boolean.FromBool(left.value != right.value);

            default:
                return new Error($"unsupported operation with numbers: {type}. Consider doing a type transformation");
        }
    }


    private Object EvaluateNumericForLoop(Object start, Object end, Object step, Block block, string name)
    {
        if (!start.IsNumeric())
            return new Error($"expected on start expr a numeric value, instead got a {start.GetType()}");
        if (!end.IsNumeric()) 
            return new Error($"expected on end expr a numeric value, instead got a {end.GetType()}");
        if (!step.IsNumeric()) 
            return new Error($"expected on end step a numeric value, instead got a {step.GetType()}");
        Number startNumber = start as Number;
        Number endNumber = end as Number;
        Number stepNumber = step as Number;
        context.Set(name, startNumber);
        for (double i = startNumber.value; i < endNumber.value; i += stepNumber.value)
        {
            startNumber.value = i;
            Object possibleError = EvaluateNode(block);
            if (possibleError.IsError()) return possibleError;
        }
        
        return Null.NULL;
    }
}
