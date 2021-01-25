using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    public float minPosY;
    public float maxPosY;
    private float offset;
    public float maxOscilation;
    // Start is called before the first frame update
    void Start()
    {
        offset = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(this.transform.position.x, offset +  Mathf.Sin(Time.time * 2) * maxOscilation, this.transform.position.z);
        this.transform.position = pos;
    }
}
