using System;
using System.Collections;
using System.Collections.Generic;
using interactor;
using UnityEngine;


/// <summary>
/// Subscription Interface
///
///
/// Don't make Unity API calls here, make your own internal state and change that and wait for the update, this is called
/// on another threads
///
/// This ain't working still so don't bother
/// </summary>
/// <deprecated>
/// ------- DEPRECATED -------
/// </deprecated>
public interface ISubscriberRunner
{
    void OnFinishExecution( /* TODO: Put here as params the object returned by user and error class */);
    void OnStartExecution();
    void OnStartTests();
}

/// <summary>
/// Runs code, and checks that everything is OK
/// </summary>
public class Runner : MonoBehaviour
{
    private static List<ISubscriberRunner> subscribers = new List<ISubscriberRunner>();

    private Object lastReturnedObject;

    private TestCase[] cases;

    private int currentCase = -1;

    private Function currentTestingFunction;

    /// <summary>
    /// Last Returned Object on the tests. This will contain an error if there is some.
    /// </summary>
    public Object LastReturnedObject => lastReturnedObject;
    
    public enum RunnerState
    {
        Waiting,
        Running,
        CheckingTests,
        NextTest
    }


    public RunnerState state = RunnerState.Waiting;  

    private float _localTimer = 0.0f;
    [SerializeField]
    private float _maxTimeExecution = 10.0f;

    private void Awake()
    {
        subscribers = new List<ISubscriberRunner>();
    }
    
    void Update()
    {
        Action action = Interactor.GetAction();
        if (action != null && action.Type == ActionType.FinishedExecution)
        {
            _localTimer = 0.0f;
        }
        if (state == RunnerState.NextTest)
        {
            _localTimer = 0.0f;
            NextCase();
            return;
        }
        if (state == RunnerState.Waiting)
        {
            _localTimer = 0.0f;
            return;
        }
        _localTimer += Time.deltaTime;

        if (_localTimer > _maxTimeExecution)
        {
            CodeExecutor.CancelExecutionTimeLimitExceeded();
            _localTimer = 0.0f;
            state = RunnerState.Waiting;
        }
    }

    
    /// <summary>
    /// Executes code related to the  current  problem in the ProblemManager
    ///
    /// Will test the outputs
    /// 
    /// Will inform all subscribers about these vents
    /// </summary>
    /// <param name="code"></param>
    /// <param name="useReturnedFunction">if it should use a returned function by the user</param>
    public void ExecuteViaProblem(string code, bool useReturnedFunction = true)
    {
        currentCase = -1;
        CodeExecutor codeExecutor = new CodeExecutor(node =>
        {
            if (node is Error)
            {
                lastReturnedObject = node;
                InformSubscribersFinish();
                return;
            }
            
            if (!useReturnedFunction)
            {
                InformSubscribersStartTests();
                state = RunnerState.Waiting;
                lastReturnedObject = node;
                return;
            }

            if (node is Function fn)
            {
                state = RunnerState.CheckingTests;
                currentTestingFunction = fn;
                cases = ProblemManager.Instance.CurrentProblem.Cases;
                NextCase();
                return;
            }
            InformSubscribersFinish();
        });
        codeExecutor.Execute(code);
        InformSubscribersStartExecution();
        state = RunnerState.Running;
        // TODO: Check tests of a problem
    }

    private void NextCase()
    {
        currentCase++;
        if (currentCase >= cases.Length)
        {
            state = RunnerState.Waiting;
            return;
        }
        state = RunnerState.Running;
        TestCase testCase = cases[currentCase];
        CodeExecutor.CallFunction(currentTestingFunction, testCase.InputData, node =>
            {
                if (node is Error)
                {
                    state = RunnerState.Waiting;
                    lastReturnedObject = node;
                    return;
                }
                if (testCase.Test(node))
                {
                    state = RunnerState.NextTest;
                    lastReturnedObject = node;
                    return;
                }
                state = RunnerState.Waiting;
                lastReturnedObject = new Error($"Bad Test Case: #{currentCase + 1}\nExpected: {Object.FromJsonString(testCase.JsonData)}\nGot:{node}");
            });
    }

    private void InformSubscribersStartTests()
    {
        for (int i = 0; i < subscribers.Count; i++)
        {
            subscribers[i].OnStartTests();
        }
    }

    private void InformSubscribersFinish()
    {
        for (int i = 0; i < subscribers.Count; i++)
        {
            subscribers[i].OnFinishExecution();
        }
    }

    private void InformSubscribersStartExecution()
    {
        for (int i = 0; i < subscribers.Count; i++)
        {
            subscribers[i].OnStartExecution();
        }
    }
    
    public static void Subscribe(ISubscriberRunner sub)
    {
        subscribers.Add(sub);
    }
}
