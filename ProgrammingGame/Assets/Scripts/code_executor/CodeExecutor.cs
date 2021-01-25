
using System.Threading;
using interactor;
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
            Debug.Log("Starting to run some code...");
            Eval ev = new Eval();
            Parser parser = new Parser(code, new Parser.Settings());
            try
            {
                Block statements = parser.Read();
                Eval.State = Eval.EvalState.Running;
                Object node = ev.EvaluateNode(statements);
                Interactor.Do(ActionType.FinishedExecution, new []{node});
                eventToRun(node);
            }
            catch (ParserException excpt)
            {
                Eval.State = Eval.EvalState.Stopped;
                eventToRun(new Error($@"syntax error on line: {excpt.Line} and column: {excpt.Column} 
                    - {excpt.Message}"));
                Debug.LogWarning(
                    $@"syntax error on line: {excpt.Line} and column: {excpt.Column} 
                    - {excpt.Message}
            ");
            }
            catch (TokenizerException tokenizer)
            {
                Eval.State = Eval.EvalState.Stopped;
                eventToRun(new Error(tokenizer.Message));
            }
        }).Start();
    }

    /// <summary>
    /// Evaluate a function returned by the user
    /// </summary>
    /// <param name="func"></param>
    /// <param name="parameters"></param>
    /// <param name="toRunWhenFinished"></param>
    /// <returns></returns>
    public static void CallFunction(Caller func, Object[] parameters = null, CodeExecutorEvent toRunWhenFinished = null)
    {
        Eval.State = Eval.EvalState.Running;
        new Thread(() =>
        {
            parameters = parameters ?? new Object[] { };
            Object obj = func.Call(parameters);
            Interactor.Do(ActionType.FinishedExecution, new Object[] {});
            // ReSharper disable once UseNullPropagation
            if (toRunWhenFinished != null)
            {
                toRunWhenFinished(obj);
            }
        }).Start();
    }

    /// <summary>
    /// Clean way of notifying from a component to the CodeExecutor that it time limit exceeded.
    /// </summary>
    public static void CancelExecutionTimeLimitExceeded()
    {
        Eval.State = Eval.EvalState.TimeLimitExceeded;
    }
}
