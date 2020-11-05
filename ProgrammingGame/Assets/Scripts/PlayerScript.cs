using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    #region Public variables
    public float speed;
    #endregion

    #region Private variables
    private Rigidbody rb;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        MoveHorizontal(10, 1, 100);
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MoveHorizontal (float speed, int direction, float time)
    {
        //direction = 1 means that it'll move right direction = -1 means it'll move left
        float _time = time;
        _time -= Time.deltaTime;
        if (time >= 0)
        {
            Debug.LogError("Time can't be negative");
            return;
        }
        while (_time >= 0)
        {
            if (direction == 1)
            {
                this.transform.position = new Vector3(this.transform.position.x + speed * Time.deltaTime, this.transform.position.y, this.transform.position.z);
            }
            if (direction == -1)
            {
                this.transform.position = new Vector3(this.transform.position.x - speed * Time.deltaTime, this.transform.position.y, this.transform.position.z);
            }
        }
        Debug.Log("Time done");
    }
}
