using System.Collections;
using System.Collections.Generic;
using interactor;
using UnityEngine;

/// <summary>
/// All kind of action types
/// </summary>
public enum ActionType
{
    /// <summary>
    /// Move will move the player in the desired vector passed by the user
    /// </summary>
    /// <param>Vector passed by the user</param>
    Move,
    
    /// <summary>
    /// Every time the code evaluates a line, it will call this action 
    /// </summary>
    /// <param>Number of the new line</param>
    NewLineOfCode,
    
    /// <summary>
    /// Finished execution CURRENT context execution.
    ///
    /// Doesn't guarantee that the code will start running soon.
    /// This should be just as a notification, but not as an event listener logic,
    /// use the callback of CodeExecutor instead for more clear program logic.
    ///
    /// </summary>
    /// <param>The object returned by the user.</param>
    FinishedExecution,
    
    Print,
    
    RunEditor,
    
    /// <summary>
    /// Tells from the terminal that the user wants to test the current code that is in the terminal
    ///
    /// 
    /// </summary>
    RunProblem,
    
    /// <summary>
    /// Checks near the user player if there is an enemy, and in that case, open the problem (set as CurrentProblem)
    /// </summary>
    OpenProblem,
    
    /// <summary>
    /// Delete current problem from the CurrentProblem
    /// </summary>
    CloseProblem,
    
    CloseEditor,
    
    OpenEditor,
    
    GetProblem,
}

/// <summary>
/// An action contains all the data that was sent from the other thread
/// Like the action that must do the game, the parameters passed by the user
/// and the interface to answer the other thread safely
/// </summary>
public class Action
{
    private ActionType type;
    private Object[] parameters;
    private Interactor.InteractorTask<Object> interactor;
    private bool finished = false;
    
    
    public ActionType Type
    {
        get => type;
    }

    public Object[] Parameters => parameters;

    public Interactor.InteractorTask<Object> Interactor => interactor;

    private SetResponse responser;

    public Action(ActionType type, Object[] parameters, Interactor.InteractorTask<Object> interactor, SetResponse responser)
    {
        this.type = type;
        this.parameters = parameters;
        this.interactor = interactor;
        this.responser = responser;
    }

    /// <summary>
    /// Answer the other thread. Keep in mind to forget about this action when calling this.
    /// </summary>
    /// <param name="response"></param>
    public void Answer(Object response)
    {
        if (finished) return;
        responser(response);
        finished = true;
    }
}
