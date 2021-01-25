using System;
using System.Collections;
using System.Collections.Generic;
using interactor;
using UnityEngine;


/// TODO
/// Do a method called ExecuteProblemDataStructure which will
/// be a special case which uses a hash map returned by the user

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
///
/// This is the main class and component that the game will use for checking how
/// user code is going.
///
///
/// When you wanna run a problem get this component and
/// run the problem.
/// </summary>
public class Runner : MonoBehaviour
{
    private static List<ISubscriberRunner> subscribers = new List<ISubscriberRunner>();

    
    /// <summary>
    /// Will be filled on `Waiting`
    ///
    /// If there is an error it will appear here
    ///
    /// If there is success it will appear here
    /// </summary>
    private Object lastReturnedObject;

    /// <summary>
    /// Current problem test cases (internal logic
    /// </summary>
    private TestCase[] cases;

    private int currentCase = -1;

    private Function currentTestingFunction;

    /// <summary>
    /// Will be filled on `Waiting`
    ///
    /// If there is an error it will appear here
    ///
    /// If there is success it will appear here
    /// </summary>
    public Object LastReturnedObject => lastReturnedObject;
    
    public enum RunnerState
    {
        /// <summary>
        /// Waiting: waits for more input
        /// </summary>
        Waiting,
        
        /// <summary>
        /// Is running the main code
        /// </summary>
        Running,
        
        /// <summary>
        /// Is checking for tests (still running code)
        /// </summary>
        CheckingTests,
 
        /// <summary>
        /// Checking next test! (still running code)
        /// </summary>
        NextTest
    }


    public RunnerState state = RunnerState.Waiting;  

    private float _localTimer = 0.0f;
    
    
    /// <summary>
    /// Max time that will take executing code of the user in seconds
    /// </summary>
    [SerializeField]
    private float _maxTimeExecution = 10.0f;

    private PipelineRunner pipelineRunner;
    
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

        if (pipelineRunner != null && pipelineRunner.Ready && !pipelineRunner.Finished)
        {
            pipelineRunner.Run();
        }

        if (pipelineRunner != null && pipelineRunner.Finished && RunnerState.NextTest == state)
        {
            _localTimer = 0.0f;
            if (pipelineRunner.LastObject is Error err)
            {
                lastReturnedObject = new Error($"Error on test case: {currentCase+1}\n{err.Message}");
                state = RunnerState.Waiting;
                return;
            }
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
    /// Will get the function returned by the user and start the test cases
    /// 
    /// </summary>
    /// <param name="code"></param>
    /// <param name="useReturnedFunction">if it should use a returned function by the user</param>
    public void ExecuteViaProblem(string code, bool useReturnedFunction = true)
    {
        // Reset current case
        currentCase = -1;
        
        // Init main codeExecutor
        CodeExecutor codeExecutor = new CodeExecutor(node =>
        {
            // Check for errors
            if (node is Error)
            {
                state = RunnerState.Waiting;
                lastReturnedObject = node;
                return;
            }
            
            // Special case that we won't use
            if (!useReturnedFunction)
            {             
                state = RunnerState.Waiting;
                lastReturnedObject = node;
                return;
            }
            
            // Now we are talking, this is the most normal case.
            if (node is Function fn)
            {
                state = RunnerState.CheckingTests;
                currentTestingFunction = fn;
                cases = ProblemManager.Instance.CurrentProblem.Cases;
                NextCase();
            }
            else
            {
                state = RunnerState.Waiting;
                lastReturnedObject = new Error($"expected a function for testcases, got {node.Type()}");
                return;
            }
        });
        codeExecutor.Execute(code);
        lastReturnedObject = null;
        state = RunnerState.Running;
    }

    /// <summary>
    /// Goes to the next test case and will set the
    /// runner state on waiting if it's the last.
    /// </summary>
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
                    pipelineRunner = new PipelineRunner(testCase, node);
                    lastReturnedObject = node;
                    return;
                }
                state = RunnerState.Waiting;
                lastReturnedObject = new Error($"Bad Test Case: #{currentCase + 1}\nExpected: {Object.FromJsonString(testCase.JsonData)}\nGot:{node}");
            });
    }
    
}
