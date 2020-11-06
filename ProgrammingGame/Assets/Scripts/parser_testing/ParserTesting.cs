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

            fact(1)
        ";
        
        
        ev = new Eval();
        var parser = new Parser(code, new Parser.Settings());
        var statements = parser.Read();
        ev.EvaluateNode(statements);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
