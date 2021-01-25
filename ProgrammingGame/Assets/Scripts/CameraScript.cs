using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Temporal camera script
        this.transform.LookAt(player);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, player.transform.position.z);
    }
}
