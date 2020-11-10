﻿// ReSharper disable PossibleNullReferenceException
// ReSharper disable CompareOfFloatsByEqualityOperator

using System;
using System.Linq;
using interactor;
using Relua.AST;
using UnityEngine;
using static Stdlib.Stdlib;

/// <summary>
/// Eval evaluates a Lua AST on the run
/// </summary>
public class Eval
{
    private int currentLine = 1;

    private int CurrentLine
    {
        get => currentLine;
        set
        {
            currentLine = value;
            Interactor.Do(ActionType.NewLineOfCode, new[] {new Number(currentLine)}).WaitInteraction();
        }
    }
    
    private Context context;
    
    public Eval(Context ctx = null)
    {
        context = ctx ?? GetStandardLibrary();
    }


    private Object EvalExpr(IExpression expr)
    {
        if (expr == null)
        {
            return Null.NULL;
        }

        // This happens when the node refers to a identifier previously parsed.
        if (expr is Node node && currentLine < node.Line && !(expr is BoolLiteral) && !(expr is Variable))
        {
            CurrentLine = node.line;
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

            case Expressions.TableConstructor:
            {
                TableConstructor table = expr as TableConstructor;
                Object[] objects = new Object[table.Entries.Count];
                Object[] keys = new Object[table.Entries.Count];
                bool isArray = true;
                for (int i = 0; i < table.Entries.Count; ++i)
                {
                    Object key   = EvalExpr(table.Entries[i].Key);
                    Object value = EvalExpr(table.Entries[i].Value);
                    objects[i] = value;
                    keys[i] = key;
                    if (!(key is Number))
                    {
                        isArray = false;
                    }
                }
                if (isArray) return new ArrayObject(objects.ToList());
                Table tableObject = new Table();
                for (int i = 0; i < table.Entries.Count; ++i)
                {
                    tableObject.Set(keys[i], objects[i]);
                }
                return tableObject;
            }

            case Expressions.TableAccess:
            {
                TableAccess ta = expr as TableAccess;
                Object idx = EvalExpr(ta.Index);
                Object variable = EvalExpr(ta.Table);
                if (idx.IsError()) return idx;
                if (variable.IsError()) return variable;
                if (variable is String str)
                {
                    if (idx is Slice sliceStr)
                    {
                        if (str.Value.Length < sliceStr.End || 0 > sliceStr.Start || sliceStr.Start > sliceStr.End)
                            return new Error(
                                $"bad slice formation in string: {sliceStr} with length {str.Value.Length}");
                        return new String(str.Value.Substring(sliceStr.Start, sliceStr.End - sliceStr.Start));
                    }
                    Object idxNumber = idx.ToNumber();
                    if (idxNumber.IsError()) return idxNumber;
                    int i = (int) (idxNumber as Number).value;
                    if (i >= str.Value.Length || i < 0)
                        return new Error($"string access out of bounds: length {str.Value.Length} idx: {i}");
                    return new String(str.Value[i].ToString());
                }

                if (variable is ArrayObject arr && arr.array.Count == 0)
                {
                    return Null.NULL;
                }
                if (variable is Table table)
                {
                    return table.Get(idx);
                }
                if (idx is Slice slice)
                {
                    return (variable as ArrayObject).Slice(slice.Start, slice.End);
                }
                Object idxN = idx.ToNumber();
                if (idxN.IsError()) return idxN;
                if (variable == Null.NULL || variable.IsError()) return variable;
                return (variable as ArrayObject).Get((int) (idxN as Number).value);
            }

            case Expressions.StringLiteral:
            {
                StringLiteral str = expr as StringLiteral;
                return new String(str.Value);
            }

            case Expressions.UnaryOp:
            {
                UnaryOp unaryOp = expr as UnaryOp;
                Object rightVal = EvalExpr(unaryOp.Expression);
                return EvalUnaryOp(rightVal, unaryOp.Type);
            }
            
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

            case Expressions.BoolLiteral:
            {
                return Boolean.FromBool((expr as BoolLiteral).Value);
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
        if (statement is Node node && currentLine < node.Line)
        {
            // Debug.Log($"line: {node.line} in {node}");
            // Wh
            CurrentLine = node.line;
        }

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
                    if (result.Type() != ReturnHolder.Name)
                    {
                        if (j >= assignment.Targets.Count) return new Error("too few variables in destructuring");
                        IAssignable assignable = assignment.Targets[j];
                        Object possibleErr = EvalAssignable(assignable, result);
                        if (possibleErr.IsError()) return possibleErr;
                        j++;
                        continue;
                    }
                    ReturnHolder returnHold = result as ReturnHolder;
                    foreach (Object returnVal in returnHold.Returns)
                    {
                        if (j >= assignment.Targets.Count) return new Error("too few variables in destructuring");
                        IAssignable assignable = assignment.Targets[j];
                        Object possibleErr = EvalAssignable(assignable, returnVal);
                        if (possibleErr.IsError()) return possibleErr;
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
                if (EvalExpr(ifStmt.MainIf.Condition).ToBool().Value)
                {
                    return EvaluateNode(ifStmt.MainIf.Block);
                }
                
                if (ifStmt.ElseIfs != null)
                {
                    foreach (ConditionalBlock condition in ifStmt.ElseIfs)
                    {
                        if (EvalExpr(condition.Condition).ToBool().Value)
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
                    if (parameters[i].Type() == Error.Name)
                    {
                        return parameters[i];
                    }
                }
                if (function.Type() != Function.Name && function.Type() != StdLibFunc.Name)
                {
                    return new Error($"function call is not a function");
                }
                Caller fn = function as Caller;
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
    
    private Object EvalAssignable(IAssignable assignable, Object target)
    {
        if (assignable == null) return Null.NULL;
        if (target.IsError()) return target;
        
        switch (assignable.GetType().Name)
        {
            case Assignables.Variable:
            {
                Variable variable = assignable as Variable;
                context.Set(variable.Name, target);
                break;
            }

            case Assignables.TableAccess:
            {
                TableAccess acc = assignable as TableAccess;
                Object idx = EvalExpr(acc.Index);
                if (idx.IsError()) return idx;
                Object value = EvalExpr(acc.Table);
                if (value == null) return new Error("unexpected error accessing table");
                if (value is ArrayObject arr && arr.array.Count > 0)
                {
                    if (idx is Slice)
                    {
                        return new Error("you can't use a slice for assignable table access");
                    }
                    idx = idx.ToNumber();
                    if (!idx.IsNumeric())
                    {
                        return new Error($"couldn't pass index {idx} to numeric value or slice");
                    }

                    Object possibleErr = (value as ArrayObject).Set((int) (idx as Number).value, target);
                    if (possibleErr.IsError()) return possibleErr;
                    return Null.NULL;
                }
                // Convert to hashTable

                if (value is Table table)
                {
                    
                    table.Set(idx, target);
                    return Null.NULL;
                }

                if (value is String)
                {
                    return new Error("strings are immutable by default");
                }

                return Null.NULL;
            }
            
            default:
                Debug.LogWarning($"unknown assignable: {assignable.GetType().Name}");
                break;
        }
        return Null.NULL;
    }

    private Object EvalUnaryOp(Object right, UnaryOp.OpType type)
    {
        switch (type)
        {
            case UnaryOp.OpType.Negate:
            {
                if (!right.IsNumeric()) 
                    return new Error($"expected numeric value on unary op, got={right.Type()}");
                (right as Number).value = -(right as Number).value;
                return right;
            }
            
            case UnaryOp.OpType.Invert:
            {
                return right.ToBool().Bang();
            }

            case UnaryOp.OpType.Length:
            {
                switch (right)
                {
                    case String str:
                        return new Number(str.Value.Length);
                    case ArrayObject arr:
                        return new Number(arr.array.Count);
                    default:
                        return new Error($"can't get length of type {right.Type()}");
                }
            }

            default:
            {
                Debug.LogWarning("unknown unary op type");
                break;
            }
        }
        return Null.NULL;
    } 
    
    public Object EvaluateNode(Node node)
    {
        if (node == null)
        {
            return Null.NULL;
        }
        if (currentLine < node.Line)
        {
            // Debug.Log($"line: {node.line} in {node}");
            // Wh
            CurrentLine = node.line;
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
        if (type == BinaryOp.OpType.Concat)
        {
            return EvaluateConcatOp(left, right);
        }
        
        if (left is Number || right is Number)
        {
            left = left.ToNumber();
            right = right.ToNumber();
            if (left.IsNumeric() && right.IsNumeric())
            {
                return EvaluateNumberOp(left as Number, right as Number, type);
            }
            return new Error($@"
                Unsupported different types in {type} operation
                left: {left}
                right: {right}
            ");    
        }

        return new Error($"can't operate between {left} and {right} on {type}");
    }

    private Object EvaluateConcatOp(Object left, Object right)
    {
        if (right is Slice slice)
        {
            switch (left)
            {
                case ArrayObject s:
                    return s.Slice(slice.Start, slice.End);
            }

            return new Error($"can't slice left variable of type {left.Type()}");
        }
        if (left.Type() != right.Type())
        {
            return new Error($"can't concatenate different types {left.Type()} and {right.Type()}");
        }

        switch (left)
        {
            case String s:
                return new String(s.Value + (right as String).Value);
            case Number number:
                return new Slice(number, right as Number);
            case ArrayObject arr:
                return arr.Concatenate(right as ArrayObject);
            default:
                return new Error($"can't operate on {left.Type()}s");
        }
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
            return new Error($"expected on start expr a numeric value, instead got a {start.Type()}");
        if (!end.IsNumeric()) 
            return new Error($"expected on end expr a numeric value, instead got a {end.Type()}");
        if (!step.IsNumeric()) 
            return new Error($"expected on end step a numeric value, instead got a {step.Type()}");
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
