using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Expressions
{
    
    public const string FunctionDefinition = "FunctionDefinition";
    
    public const string Variable = "Variable";
    
    public const string NumberLiteral = "NumberLiteral";

    public const string UnaryOp = "UnaryOp";

    public const string BoolLiteral   = "BoolLiteral";
    
    public const string StringLiteral = "StringLiteral";
    
    public const string TableConstructor = "TableConstructor";
    
    public const string TableAccess = "TableAccess";
}

public static class Statements
{
    public const string Assignment = "Assignment";

    public const string FunctionCall = "FunctionCall";
    
    public const string If = "If";

    public const string Return = "Return";

    public const string BinaryOp = "BinaryOp";

    public const string NumericFor = "NumericFor";

}

public static class Assignables
{
    public const string Variable = "Variable";

    public const string TableAccess = "TableAccess";
}