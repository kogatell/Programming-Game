using System.Linq;
using interactor;
using UnityEngine;

public class TerminalLib : Lib
{
	private const string HELP_MESSAGE = @"Welcome to the terminal! Here is where you will call commands that will do
		cool things, here are some examples!
		- Write move(UP or DOWN or RIGHT or LEFT) to move somewhere
		- Write `talk()` near an enemy, it might say something useful to you!
		- Write `solve()` to solve the problem of an enemy (you must be near it)
		- Write `print()` and pass inside the parenthesis the things that you wanna show on the terminal
		- Write `import('name of the lib')` to import the library you want
		
		Available libraries:		
		- Hash Map `hashmap`
		- Math Library `math`
		- Array Library `array`
		
		To get help on how to use them:
		- Write `library_help('name of the lib')`


		INTERESTING INFORMATION:

		To do type conversion between numbers or strings:
		- `string(...)` and `number(...)` will do what is expected
		
		To create an empty hash table:
		- `table()`

		To show a declared variable on the terminal:
		- `>> return variable;`			
		
		When you don't remember something, write `tutorial('name of tutorial!')` 
		and write inside the parenthesis what do you need to know about!
		Don't be afraid from the big list of things, when you advance through the game and talk to the enemies,
		you will be shown tutorials and information! Consider this more as a remember glossary
		List of tutorials
		- variables
		- returns
		- operations
		- strings
		- functions
		- arrays
		- for loops
		";
	
	public override Context InjectLibrary(Context context)
    {
		context.Set("help", new StdLibFunc(parameters => new String(HELP_MESSAGE)));
		context.Set("run_code", new StdLibFunc(Run));
		context.Set("library_help", new StdLibFunc(LibraryHelp));
		context.Set("run_problem", new StdLibFunc((_) =>
		{
			Interactor.Do(ActionType.RunProblem, null);
			return new String("running test cases...");
		}));
		context.Set("open_editor", new StdLibFunc((_) =>
		{
			Interactor.Do(ActionType.OpenEditor, null);
			return new String("opened editor...");
		}));
		context.Set("close_editor", new StdLibFunc((_) =>
		{
			Interactor.Do(ActionType.CloseEditor, null);
			return new String("closed editor...");
		}));
		context.Set("move", new StdLibFunc((parameters) =>
		{
			if (parameters.Length != 1 || !parameters[0].IsNumeric())
			{
				return new Error("expected one parameter only for move, the cell number value of where you wanna move");
			} 
			return Interactor.Do(ActionType.Move, parameters).WaitInteraction();
		}));
		context.Set("get_problem", new StdLibFunc(_ => 
			Interactor.Do(ActionType.GetProblem, null).WaitInteraction())
		);
		return context;
    }
	
	private static Object Run(Object[] _)
	{
		Interactor.Do(ActionType.RunEditor, null);
		return new String("Successfully ran!");
	}
	
	private static Object LibraryHelp(Object[] parameters)
	{
		if (parameters.Length != 1)
		{
			return new Error("library_help only accepts 1 parameter (string)");
		}
		if (!(parameters[0] is String str))
		{
			return new Error("parameter 1 should be a string on library");
		}

		string libName = str.Value;
		Lib lib = Libraries.Get(libName);
		if (lib == null)
		{
			return new Error($"library `{libName}` not found");
		}
		return new String(lib.Documentation.Aggregate("", (acc, doc) => $"{acc}{doc}"));
	}
}
