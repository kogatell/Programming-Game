using System.Linq;
using UnityEngine;

public class StringLib : Lib
{
	#region Private Variables

	#endregion

	#region Public Variables

	#endregion

	#region Properties

	#endregion

	#region MonoBehaviour

    private void Start()
    {
        
    }

	private void Update()
    {
        
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    #endregion

    public override Context InjectLibrary(Context ctx)
    {
	    ctx.Set("char_code", new StdLibFunc(param =>
	    {
		    if (param.Length != 1) return new Error("char_code expects 1 parameter to be a string");
		    if (!(param[0] is String str))
		    {
			    return new Error("char_code expects 1 parameter to be a string");
		    }
		    char c = str.Value[0];
		    return new Number(c);
	    }));

	    ctx.Set("lowercase", new StdLibFunc(param =>
	    {
		    if (param.Length != 1) return new Error("lowercase expects 1 parameter to be a string");
		    if (!(param[0] is String str))
		    {
			    return new Error("lowercase expects 1 parameter to be a string");
		    }
		    return new String(str.Value.ToLower());
	    }));
	    
	    ctx.Set("uppercase", new StdLibFunc(param =>
	    {
		    if (param.Length != 1) return new Error("uppercase expects 1 parameter to be a string");
		    if (!(param[0] is String str))
		    {
			    return new Error("uppercase expects 1 parameter to be a string");
		    }
		    return new String(str.Value.ToUpper());
	    }));
	    
	    ctx.Set("split", new StdLibFunc(param =>
	    {
		    if (param.Length != 2) return new Error("split expects 2 parameter to be strings");
		    if (!(param[0] is String str))
		    {
			    return new Error("split expects 1st parameter to be a string");
		    }
		    if (!(param[1] is String split))
		    {
			    return new Error("split expects 2nd parameter to be a string");
		    }
		    string[] s = str.Value.Split(split.Value.ToCharArray());
		    Object[] strings = new Object[s.Length];
		    return new ArrayObject(strings.ToList());
	    }));
	    
	    return ctx;
    }
}
