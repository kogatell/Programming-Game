using System;
using UnityEngine;

/// <summary>
/// Libraries are static references to libraries that are available everytime
/// and that can be imported with import("libname")
///
/// We are creating this because is more obvious for us to create a mechanic of library exploring
/// and that the user is able to import any library at any moment everywhere, also we can implement
/// here features like library searching and we don't have to import every library that matters
/// on every problem, if we need a BinaryTree definition on a problem we will use on that case a
/// personalized library on the problem like we wanted before.
/// </summary>
public class Libraries : MonoBehaviour
{
	#region Private Variables

	[SerializeField] private Lib[] libs;
	private static Libraries instance;
	#endregion

	#region Public Variables

	 
	#endregion

	#region Properties

	public Libraries Instance => instance;
	
	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		instance = this;
	}

	#endregion

    #region Public Methods

    public static Lib Get(string libName)
    {
	    foreach (Lib l in instance.libs)
	    {
		    if (l.LibName == libName) return l;
	    }

	    return null;
    }
    #endregion

    #region Private Methods

    #endregion
}
