using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Expressions
{
    
    public const string FunctionDefinition = "FunctionDefinition";
    public const string Variable = "Variable";
    public const string NumberLiteral = "NumberLiteral";
    

}

public static class Statements
{
    public const string Assignment = "Assignment";

    public const string FunctionCall = "FunctionCall";
    
    public const string If = "If";

    public const string Return = "Return";

    public const string BinaryOp = "BinaryOp";

}

public static class Assignables
{
    public const string Variable = "Variable";
}