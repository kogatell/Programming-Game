using UnityEngine;

public class PipelineRunner
{

	private Object lastObject = Null.NULL;

	public Object LastObject => lastObject;

	private TestCase testcase;

	private bool finished;

	private bool ready;

	public bool Ready => ready;

	public bool Finished => finished;
	
	private int currentPipeline = -1;

	private Object[] pipeline;

	private Table instance;
	
	public PipelineRunner(TestCase testcase, Object objectToBeEvaluated)
	{
		if (!testcase.IsInstance)
		{
			finished = true;
			return;
		}
		this.testcase = testcase;
		pipeline = testcase.Pipeline;
		finished = false;
		ready = true;
		instance = objectToBeEvaluated as Table;
		IsGood(instance);
	}

	public void Run()
	{
		if (finished) return;
		if (!ready) return;
		// Go to next pipeline
		currentPipeline++;
		
		// If finished pipelining set ready to true to indicate
		// the Runner class that we finished
		if (currentPipeline >= pipeline.Length)
		{
			finished = true;
			ready = true;
			return;
		}
		
		// ready to false because we wanna make sure that we are not finished to the outsiders
		ready = false;
		
		// get current pipeline info (the call, the func (if it is a function), and the expected)
		ArrayObject pipelineInfo = pipeline[currentPipeline] as ArrayObject;
		
		// if not good just finish
		if (!IsGood(pipelineInfo))
		{
			return;
		}
		
		// attName is the attribute we wanna access
		String attName = pipelineInfo.array[0] as String;
		if (!IsGood(attName, "ERR"))
		{
			return;
		}
		
		// check if it is a function
		bool isFunc = pipelineInfo.array[1] != Null.NULL;
		
		// parameters unwrap
		ArrayObject parameters = isFunc ? pipelineInfo.array[1] as ArrayObject : new ArrayObject();
		
		// expected unwrap
		Object expected = pipelineInfo.array[2];
		
		// get the object returned by the user and access the attribute
		Object obj = instance.Get(attName);
		
		// if it is a function
		if (isFunc)
		{
			// not a function, the user didn't meet expectations
			if (!(obj is Caller))
			{
				IsGood(null, $"expected to call something in {attName} at instance {instance}, but found a {obj.Type()}");
				return;
			}
			
			// Call the codeExecutor and pass the parameters to the function
			CodeExecutor.CallFunction(obj as Caller, parameters.array.ToArray(), (node) =>
			{
				if (node is Error)
				{
					finished = true;
					lastObject = node;
					return;
				}
				// check deep equal
				if (node.EqualDeep(expected))
				{
					ready = true;
				}
				else
				{
					finished = true;
					lastObject = new Error($"Pipeline {currentPipeline+1} with parameters: {parameters}\n failed, on function call {attName}, expected: {expected}. Got: {node}");
				}
			});
			return;
		}

		if (!obj.EqualDeep(expected))
		{
			finished = true;
			lastObject = new Error($"Pipeline {currentPipeline+1} with parameters: {parameters} failed, on attribute {attName} expected: {expected}. Got: {obj}");
		}
		else
		{
			ready = true;
		}
	}

	private bool IsGood(Object obj, string msg = "bad test input")
	{
		if (obj == null)
		{
			finished = true;
			lastObject = new Error(msg);
			return false;
		}
		return true;
	}
	
}
