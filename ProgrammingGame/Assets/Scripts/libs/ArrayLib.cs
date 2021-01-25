using System.Linq;
using UnityEngine;

public class ArrayLib : Lib
{
	public override Context InjectLibrary(Context context)
	{
		Table array = new Table();
		array.Set(new String("sort"), new StdLibFunc(Sort));
		array.Set(new String("ascendant"), new StdLibFunc(Ascendant));
		array.Set(new String("descendent"), new StdLibFunc(Descendent));
		array.Set(new String("append"), new StdLibFunc(Push));
		array.Set(new String("pop"), new StdLibFunc(Pop));
		array.Set(new String("remove_at"), new StdLibFunc(RemoveAt));
		array.Set(new String("map"), new StdLibFunc(Map));
		array.Set(new String("filter"), new StdLibFunc(Filter));
		array.Set(new String("reduce"), new StdLibFunc(Reduce));
		context.Set("array", array);
		return context;
	}
	
	private static Object Reduce(Object[] parameters)
	{
		if (parameters.Length < 2 || parameters.Length > 3)
		{
			return new Error("expected 2 or 3 parameters on array.reduce function");
		} 
		Object parameter = parameters[0];
		if (!(parameter is ArrayObject arr))
		{
			return new Error($"expected array in first parameter on reduce, got {parameter.Type()}");
		}
		
		if (!(parameters[1] is Caller callable))
		{
			return new Error($"expected callable in second parameter on filter, got {parameters[1].Type()}");
		}
		
		Object init = parameters.Length == 3 ? parameters[2] : new ArrayObject();
		ArrayObject array = new ArrayObject(arr.array.ToList());
		for (int i = 0; i < array.array.Count; i++)
		{
			init = callable.Call(new[] {init, array.array[i]});
			if (init is Error) return init;
		}
		return init;
	}
	
	private static Object Filter(Object[] parameters)
	{
		if (parameters.Length != 2)
		{
			return new Error("expected 2 parameters on array.filter function");
		} 
		Object parameter = parameters[0];
		if (!(parameter is ArrayObject arr))
		{
			return new Error($"expected array in first parameter on filter, got {parameter.Type()}");
		}

		if (!(parameters[1] is Caller callable))
		{
			return new Error($"expected callable in second parameter on filter, got {parameters[1].Type()}");
		}
		ArrayObject array = new ArrayObject(arr.array.ToList());
		array.array = array.array.FindAll(element => 
			Boolean.FromObject(callable.Call(new[]{element.Clone()})).Value).ToList();
		return array;
	}

	private static Object Map(Object[] parameters)
	{
		if (parameters.Length != 2)
		{
			return new Error("expected 2 parameters on array.map function");
		} 
		Object parameter = parameters[0];
		if (!(parameter is ArrayObject arr))
		{
			return new Error($"expected array in first parameter on map, got {parameter.Type()}");
		}

		if (!(parameters[1] is Caller callable))
		{
			return new Error($"expected callable in second parameter on map, got {parameters[1].Type()}");
		}
		ArrayObject array = new ArrayObject(arr.array.ToList());
		array.array = array.array.Select(element => callable.Call(new[]{element.Clone()})).ToList();
		return array;
	}

	private static Object RemoveAt(Object[] parameters)
	{
		if (parameters.Length != 2) return new Error("expected 2 parameters in remove_at");
		Object first = parameters[0];
		Object second = parameters[0];
		if (!(first is ArrayObject arr))
		{
			return new Error($"expected array as first parameter, got {first.Type()} in remove_at");
		}

		if (!(second is Number number))
		{
			return new Error($"expected number as second parameter, got {second.Type()} in remove_at");
		}

		return arr.RemoveAt((int) number.value);
	}

	private static Object Pop(Object[] parameters)
	{
		if (parameters.Length != 1)
		{
			return new Error("expected 1 parameter to be an array on array.pop");
		}

		if (parameters[0] is ArrayObject arr)
		{
			return arr.Pop();
		}
		return new Error($"expected parameter to be an array, got {parameters[0].Type()}");
	}
	
	private static Object Push(Object[] parameters)
	{
		if (parameters.Length <= 1)
		{
			return new Error("expected 1 parameter to be an array on array.append and 2nd parameter to be any");
		}
		if (parameters[0] is ArrayObject arr)
		{
			for (int i = 1; i < parameters.Length; ++i)
			{
				arr.Append(parameters[i]);	
			}
			return Boolean.True;
		}
		return new Error($"expected parameter to be an array, got {parameters[0].Type()}");
	}

	private static Object Ascendant(Object[] parameters)
	{
		if (parameters[0] is Number first && parameters[1] is Number second)
		{
			return new Number(first.value - second.value);
		}
		return Null.NULL;
	}
	
	private static Object Descendent(Object[] parameters)
	{
		if (parameters[0] is Number second && parameters[1] is Number first)
		{
			return new Number(first.value - second.value);
		}
		return Null.NULL;
	}

	private static Object Sort(Object[] objects)
	{
		if (objects.Length != 2)
		{
			return new Error("expected 2 parameters, first one to be the array to sort, second the callback to sort");
		}

		if (objects[0] is ArrayObject arr && objects[1] is Caller callable)
		{
			Error error = null;
			arr.array.Sort((first, second) =>
			{
				Object param = callable.Call(new []{first, second});
				if (param is Number n)
				{
					return (int) n.value;
				}
				error = new Error("error sorting because the callback didn't return a number");
				return 0;
			});
			if (error != null)
			{
				return error;
			}
			return Boolean.True;
		}
		return new Error("expected 2 parameters, first one be the array to sort, second the callback to sort");
	}
}
