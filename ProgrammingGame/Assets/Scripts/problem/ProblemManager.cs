using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Problem manager will manage the problem order execution
/// </summary>
public class ProblemManager : MonoBehaviour
{
    [SerializeField] private Problem[] problems;
    
    private static ProblemManager instance;

    public static ProblemManager Instance => instance;
    
    private Problem currentProblem;
    
    public Problem CurrentProblem => currentProblem;

    public Problem[] Problems => problems;
    
    void Awake()
    {
        if (instance != null) return;
        DontDestroyOnLoad(gameObject);
        currentProblem = problems[0];
        instance = this;
    }


    /// <summary>
    /// TODO
    ///
    /// Goes to the next scene/problem.
    /// </summary>
    public void NextProblem()
    {
        
    }

}
