using System.Threading;
using Relua;
using UnityEngine;

public class LuaREPL
{
	#region Private Variables
	
	private Eval eval;
	private Lib[] libs;

	#endregion

	#region Public Variables

	#endregion

	#region Properties

	#endregion


	#region Public Methods

	public LuaREPL(Lib[] libs)
	{
		if (libs == null)
		{
			libs = new Lib[0];
		}
		Context libraries = Stdlib.Stdlib.GetStandardLibrary();
		for (int i = 0; i < libs.Length; i++)
		{
			libraries = libs[i].InjectLibrary(libraries);
		}
		this.libs = libs;
		eval = new Eval(libraries);
	}

	public Response<Object> Evaluate(string code)
	{
		Response<Object> resp = new Response<Object>();
		new Thread(() =>
		{
			try
			{
				Parser parser = new Parser(code, new Parser.Settings());
				Eval.State = Eval.EvalState.Running;
				Object response = eval.EvaluateNode(parser.Read());
				resp.SetReady(response);
			}
			catch (ParserException excpt)
			{
				resp.SetReady(new Error(excpt.Message));
			}
			catch (TokenizerException tokenizer)
			{
				resp.SetReady(new Error(tokenizer.Message));
			}
			catch
			{
				resp.SetReady(new Error("fatal error"));
			}
		}).Start();
		return resp;
	}

	public void CancelExecution()
	{
		Eval.State = Eval.EvalState.CancelledByUser;
	}
	
	#endregion

	#region Private Methods

	#endregion
}
