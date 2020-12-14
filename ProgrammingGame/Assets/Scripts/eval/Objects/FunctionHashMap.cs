using Relua.AST;
using UnityEngine;

public class FunctionHashMap : Object, Caller
{
	private Function func;
	private Table ctx;
	
	public FunctionHashMap(Function fn, Table tableCtx)
	{
		func = fn;
		ctx = tableCtx;
	}

	

	public override string Type()
	{
		return "FunctionHashMap";
	}

	public Object Call(Object[] parameters)
	{
		throw new System.NotImplementedException();
	}
}
