using System;
using Relua.AST;
using UnityEngine;

/// <summary>
/// Function that contains the `this` context
/// so the user can access the table itself inside a function 
/// </summary>
public class FunctionHashMap : Object, Caller
{
	public const string Name = "Function";
	private Function func;
	private Table ctx;
	
	public FunctionHashMap(Function fn, Table tableCtx)
	{
		func = fn;
		ctx = tableCtx;
	}

	public override string Type()
	{
		return Name;
	}

	public Object Call(Object[] parameters)
	{
		Object[] newParams = new Object[parameters.Length+1];
		newParams[0] = ctx;
		for (int i = 1; i < newParams.Length; i++)
		{
			newParams[i] = parameters[i - 1];
		}
		return func.Call(newParams);
	}
}
