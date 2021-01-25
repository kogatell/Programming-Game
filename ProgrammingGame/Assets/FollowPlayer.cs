using System;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	#region Private Variables

	[SerializeField] private Transform player;
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

	private void LateUpdate()
	{
		transform.LookAt(player);
		Vector3 vec = player.position;
		vec.y += 5f;
		vec.z -= 5f;
		transform.position = vec;
	}

	#endregion

    #region Public Methods

    #endregion

    #region Private Methods

    #endregion
}
