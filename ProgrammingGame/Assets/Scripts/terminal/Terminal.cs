using System;
using interactor;
using UnityEngine;

public class Terminal : MonoBehaviour
{
	#region Private Variables

	private LuaREPL luaRepl;

	[SerializeField] private GameObject editorGameObject;

	[SerializeField]
	private Lib[] libs;

	private TerminalUI terminal;

	private Response<Object> resp;

	private float currentTime = 0.0f;

	private float maxExecutionTime = 3f;

	private bool runningCode = false;

	private Runner runner;

	[SerializeField]
	private EditorTest editor;

	private Object returnObject;

	private bool checkingTestcases = false;

	[SerializeField] private PlayerMove pm;
	
	#endregion

	#region Public Variables
	
	#endregion

	#region Properties

	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		luaRepl = new LuaREPL(libs);
		runner = FindObjectOfType<Runner>();
	}

	private void Start()
	{
		
		terminal = FindObjectOfType<TerminalUI>(); 
        terminal.Write("Welcome to the game! Write some commands! Write `help()` to show help prompt");
        terminal.AddListener((s) =>
        {
	        if (checkingTestcases) return;
	        // Cancels execution everywhere, even if we are evaluating
	        luaRepl.CancelExecution();

	        if (resp != null && !resp.Ready) return;
	        terminal.Write(s);
	        resp = luaRepl.Evaluate(s.Substring(3));
        });
    }

	private void Update()
    {	
	    CheckRepl();
	    CheckRunningCode();
	    CheckRunner();
	    HandleActions();
    }

    #endregion

    #region Public Methods

    public void OpenProblem()
    {
	    editorGameObject.SetActive(true);
	    terminal.Write(ProblemManager.Instance.CurrentProblemHolder.Problem.Statement);
    }
    
    #endregion

    #region Private Methods

    private void CheckRunner()
    {
	    if (!checkingTestcases) return;
	    if (runner.state == Runner.RunnerState.Waiting)
	    {
		    checkingTestcases = false;
		    Object lastReturned = runner.LastReturnedObject;
		    // Unsuccessful run
		    if (lastReturned is Error error)
		    {
			    terminal.Write($"ERROR:\n{error}");
			    return;
		    }

		    if (lastReturned == null) return;
		    terminal.Write("Accepted!");
		    ProblemManager.Instance.CurrentProblemHolder.Destroy();
		    ProblemManager.Instance.CurrentProblemHolder = null;
	    }
    }

    private void CheckRepl()
    {
	    if (resp != null && resp.Ready)
	    {
		    terminal.Write(resp.Data.ToString());
		    currentTime = 0.0f;
		    resp = null;
	    }

	    if (resp != null)
	    {
		    currentTime += Time.deltaTime;
		    if (currentTime >= maxExecutionTime)
		    {
			    luaRepl.CancelExecution();
		    }
	    }
    }

    private void CheckRunningCode()
    {
	    if (runningCode)
	    {
		    currentTime += Time.deltaTime;
		    if (currentTime >= maxExecutionTime)
		    {
			    luaRepl.CancelExecution();
		    }
	    }

	    if (returnObject != null)
	    {
		    runningCode = false;
		    terminal.Write(returnObject.ToString());
		    returnObject = null;
	    }
    }

    private void HandleActions()
    {
	    Action action = Interactor.GetAction();
	    if (action != null)
	    {
		    switch (action.Type)
		    {
			    case ActionType.Print:
			    {
				    foreach (Object parameter in action.Parameters)
				    {
					    terminal.Write(parameter.ToString());    
				    }
				    action.Answer(null);
				    break;
			    }

			    case ActionType.RunEditor:
			    {
				    terminal.Write("Running script...");
				    CodeExecutor ce = new CodeExecutor(returnValue => { returnObject = returnValue; });
				    ce.Execute(editor.Code);
				    action.Answer(Boolean.True);
				    runningCode = true;
				    break;
			    }

			    case ActionType.RunProblem:
			    {
				    if (!ProblemManager.Instance.CurrentProblemHolder)
				    {
					    terminal.Write("Error: there is no selected problem, go near a robot and run: get_problem()");
					    action.Answer(Boolean.False);
					    return;
				    }
				    terminal.Write("Testing test cases...");
				    checkingTestcases = true;
				    action.Answer(Boolean.True);
				    runner.ExecuteViaProblem(editor.Code);
				    break;
			    }

			    case ActionType.OpenEditor:
			    {
					 editorGameObject.SetActive(true);
					 action.Answer(Boolean.True);
					 break;
			    }

			    case ActionType.CloseEditor:
			    {
				    editorGameObject.SetActive(false);
				    action.Answer(Boolean.True);
				    break;
			    }

			    case ActionType.Move:
			    {
				    
				    action.Answer(Boolean.FromBool(pm.WalkToPoint((int) (action.Parameters[0] as Number).Value)));
				    break;
			    }
			    
			    default:
			    {
				    break;
			    }
		    }
	    }
    }
    #endregion
}
