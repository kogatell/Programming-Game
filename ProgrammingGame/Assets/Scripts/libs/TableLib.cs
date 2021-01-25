using UnityEngine;

public class TableLib : Lib
{
	public override Context InjectLibrary(Context context)
	{
		Table hashmap = new Table();
		hashmap.Set(new String("delete"), new StdLibFunc(Delete));
		hashmap.Set(new String("keys"), new StdLibFunc(Keys));
		hashmap.Set(new String("values"), new StdLibFunc(Values));
		hashmap.Set(new String("contains"), new StdLibFunc(Contains));
		context.Set("hashmap", hashmap);
		return context;
	}

	private static Object Contains(Object[] parameters)
	{
		if (parameters.Length != 2)
		{
			return new Error($"expected 2 parameters on hashmap.contains(), got {parameters.Length}");
		}

		if (!(parameters[0] is Table t))
		{
			return new Error($"expected first parameter of hashmap.contains to be a table, got: {parameters[0].Type()}");
		}

		return Boolean.FromBool(t.Contains(parameters[1]));
	}
	

	private static Object Delete(Object[] parameters)
	{
		if (parameters.Length != 2)
		{
			return new Error("hashmap.delete expects 2 parameters only");
		}

		if (!(parameters[0] is Table table))
		{
			return new Error("first parameter of hashmap.delete should be a table");
		}

		return table.Delete(parameters[1]);
	}
	
	private static Object Keys(Object[] parameters)
	{
		if (parameters.Length != 1)
		{
			return new Error("hashmap.keys expects 1 parameter only");
		}

		if (!(parameters[0] is Table table))
		{
			return new Error("first parameter of hashmap.keys should be a table");
		}

		return table.Keys();
	}

	private static Object Values(Object[] parameters)
	{
		if (parameters.Length != 1)
		{
			return new Error("hashmap.keys expects 1 parameter only");
		}

		if (!(parameters[0] is Table table))
		{
			return new Error("first parameter of hashmap.keys should be a table");
		}

		return table.Values();
	}

}
