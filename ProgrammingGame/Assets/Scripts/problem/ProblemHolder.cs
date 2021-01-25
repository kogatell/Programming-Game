using UnityEngine;

public class ProblemHolder : MonoBehaviour
{
	#region Private Variables

    [SerializeField]
    Problem problem;
    
	#endregion

	#region Public Variables

	public Problem Problem => problem;
	
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

    public void Destroy()
    {
	    Destroy(gameObject);
    }
    
    #endregion

    #region Private Methods

    #endregion
}
