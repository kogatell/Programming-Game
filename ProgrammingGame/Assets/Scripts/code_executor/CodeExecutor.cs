
using System.Threading;
using Relua;
using Relua.AST;
using UnityEngine;

public delegate void CodeExecutorEvent(Object objectReturnedByUser);

/// <summary>
/// Class that will run code in another thread
/// </summary>
public class CodeExecutor
{

    private CodeExecutorEvent eventToRun;
    
    public CodeExecutor(CodeExecutorEvent eventToRun)
    {
        this.eventToRun = eventToRun;
    }

    public void Execute(string code)
    {
        new Thread(() =>
        {
            Eval ev = new Eval();
            Parser parser = new Parser(code, new Parser.Settings());
            try
            {
                Block statements = parser.Read();
                Object node = ev.EvaluateNode(statements);
                eventToRun(node);
                /*Debug.Log($"evaluator returned a {node.Type()} of val {node}");
                
                if (node.IsError())
                {
                    Error err = node as Error;
                    Debug.LogWarning($"program returned the following error: {err.Message}");
                }*/
            }
            catch (ParserException excpt)
            {
                Debug.LogWarning(
                    $@"syntax error on line: {excpt.Line} and column: {excpt.Column} 
                    - {excpt.Message}
            ");
            }
        }).Start();
    }
}
