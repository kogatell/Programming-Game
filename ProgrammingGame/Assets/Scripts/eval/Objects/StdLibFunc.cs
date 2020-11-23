using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate Object STDFUNC(Object[] parameters);

public class StdLibFunc : Object, Caller
{
    public const string Name = "StdLibFunc";
    
    public override string Type()
    {
        return Name;
    }

    private STDFUNC function;

    public STDFUNC Function => function;

    public StdLibFunc(STDFUNC func)
    {
        function = func;
    }

    public Object Call(Object[] objects)
    {
        return function(objects);
    }
    
}
