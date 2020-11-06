using System;
using System.Collections;
using System.Collections.Generic;
using Relua;
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
                return 1
              else
                return n * fact(n-1)
              end
            end 

            s = fact(3)
            return s
        ";
        
        
        ev = new Eval();
        var parser = new Parser(code, new Parser.Settings());
        var statements = parser.Read();
        var node = ev.EvaluateNode(statements);
        Debug.Log($"evaluator returned a {node.GetType()} of val {node}");
        if (node.IsError())
        {
            var err = node as Error;
            Debug.LogWarning($"program returned the following error: {err.Message}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
