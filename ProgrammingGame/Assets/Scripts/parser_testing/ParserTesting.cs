using System;
using System.Collections;
using System.Collections.Generic;
using Relua;
using Relua.AST;
using UnityEngine;

public class ParserTesting : MonoBehaviour
{
    private Eval ev;
    
    /// <summary>
    /// This code is just for testing
    ///
    /// you can play around the code in the code variable
    /// </summary>
    void Start()
    {
        // Code containing all the features of the language
        string code = @"
            arr = {1, 2, 3, ""4"", 5}[0 .. 4] .. { 6 } --  slice to { 1,2,3,4, 6 }
            for i=0, #arr do -- #arr gets length of the array, which is 4
                arr[i] = arr[i] * 2
            end
            function fact (n)
              if n == 0 then
                return (1 / 2) * (3 * 5 + 1) / 2 + 3
              else
                return n * fact(n-1)
              end
            end 
            s = fact(3)
            if s == 42 then
                s = s + 1
            end
            function f(c)
                return 0, 1
            end
            a = ""1"" .. ""2""[0]
            j, x, z = f(0), 1
            for i=1,10 do j = j + i end 
            something = true
            return j + s + x + z + not something + a + arr[-1]
        ";
        
        
        ev = new Eval();
        Parser parser = new Parser(code, new Parser.Settings());
        try
        {
            Block statements = parser.Read();
            Object node = ev.EvaluateNode(statements);
            Debug.Log($"evaluator returned a {node.GetType()} of val {node}");
            if (node.IsError())
            {
                Error err = node as Error;
                Debug.LogWarning($"program returned the following error: {err.Message}");
            }
        }
        catch (ParserException excpt)
        {
            Debug.LogWarning(
            $@"syntax error on line: {excpt.Line} and column: {excpt.Column} 
                    - {excpt.Message}
            ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
