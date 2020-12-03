using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using interactor;
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
        // RunMoveExample();
        RunFunctionAndCancelExecOutside();
    }

    public void RunFunctionAndCancelExec()
    {
        string code = @"                
            return function()
                print(""this should be printed"")
                cancel() 
                print(""THIS SHOULDN'T BE PRINTED"");
            end
        ";
        Function handler = null;
        CodeExecutor codeExecutor = new CodeExecutor(_ =>
        {
            handler = _ as Function;
            Debug.Log("Running function...");
            CodeExecutor.CallFunction(handler);
            Debug.Log($"Function stopped. Evaluator state: {Eval.State}");
            
        });
        codeExecutor.Execute(code);
        
    }
    
    public void RunFunctionAndCancelExecOutside()
    {
        string code = @"                
            return function()
                for i=0, 1000000000 do -- Infinite loop, so we can cancel.                    
                end
                    
                print(""THIS SHOULDN'T BE PRINTED"");
            end
        ";
        Function handler = null;
        CodeExecutor codeExecutor = new CodeExecutor(_ =>
        {
            handler = _ as Function;
            Debug.Log("Running function...");
            CodeExecutor.CallFunction(handler, null, __ =>
            {
                Debug.Log($"Finished with state: {Eval.State}");
            });
            CodeExecutor.CancelExecutionTimeLimitExceeded();

        });
        codeExecutor.Execute(code);
    }

    public void RunMoveExample()
    {
        string code = @"                
            moveTo = { 1, 2 } -- x , y
             -- move is a standard library function that we will show later how it's created
            returnValue = move(moveTo)
            print(returnValue) -- ?????
            print(test(1))
            return true
        ";
        CodeExecutor codeExecutor = new CodeExecutor(node =>
        {
            Debug.Log($"evaluator returned a {node.Type()} of val {node}");
            if (node.IsError())
            {
                Error err = node as Error;
                Debug.LogWarning($"program returned the following error: {err.Message}");
            }
            Debug.Log($"Evaluator EndState: {Eval.State}");
        });
        codeExecutor.Execute(code);
    }

    public void RunEverything()
    {
        // Code containing all the features of the language
        string code = @"
        print(move({1,2}))
        arr = {1, 2, 3, ""4"", 5}[0 .. 4] .. { 6 } --  slice to { 1,2,3,4, 6 }
        for i=0, #arr do -- #arr gets length of the array, which is 4
            arr[i] = arr[i] * 2
        end
        p = 35
        handler = { 
            data = 0, 
            handle = function(n)
                handler.data = handler.data + n; 
                print(p)
                p = p + 1
            end, 
            z = p
        } 
        print(handler)
        table = { TABLE=true, arr={1,2,3}, key=1 }
        table[""uwu""] = 1
        append(table.arr, 4)
        print(table.key)
        append(arr, 10)
        print(table)
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
        j, x, z, arr[0] = f(0), 1, 0 -- Example of destructuring. 0, 1, 1
        for i=1,10 do j = j + i end 
        something = true
        return handler
        "; // return j + s + x + z + not something + a + arr[-1] + arr[0] + table[""uwu""]

        CodeExecutor codeExecutor = new CodeExecutor(node =>
        {
            Debug.Log($"evaluator returned a {node.Type()} of val {node}");
            ExampleOfHandlingATableObjectWhichHasAHandler(node);
            if (node.IsError())
            {
                Error err = node as Error;
                Debug.LogWarning($"program returned the following error: {err.Message}");
            }
        });
        codeExecutor.Execute(code);
    }

    /// <summary>
    /// If the user returns a handler
    /// { handle=function(n) /* something */ end, data=0 } we can call handle and it will change state of the code
    /// </summary>
    /// <param name="node"></param>
    void ExampleOfHandlingATableObjectWhichHasAHandler(Object node)
    {
        if (node is Table table)
        {
            // Equivalent of
            Function fn = table.Get(new String("handle")) as Function;
            fn.Call(new[] {new Number(10)});
            fn.Call(new[] {new Number(10)});
            Debug.Log(table);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Action action = Interactor.GetAction();
        
        if (action != null && action.Type == ActionType.NewLineOfCode)
        {
            Number number = (action.Parameters[0] as Number);
            action.Answer(Null.NULL);
        }

        if (action != null && action.Type == ActionType.Move)
        {
            action.Answer(new Number(10));
        }
    }
}
