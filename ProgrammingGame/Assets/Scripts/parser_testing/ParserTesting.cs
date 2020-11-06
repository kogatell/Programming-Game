using System;
using System.Collections;
using System.Collections.Generic;
using Relua;
using Relua.AST;
using UnityEngine;

public class ParserTesting : MonoBehaviour
{
    private Eval ev;
    
    // Start is called before the first frame update
    void Start()
    {
        string code = @"
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
            j = 0
            for i=1,10 do j = j+i end
            return j
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
