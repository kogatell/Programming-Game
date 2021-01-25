using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private float timer;
    private List<GameObject> goOnCollision;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        setTimer(10);
        //MoveHorizontal(1, 1, 10);
        //Jump(1000);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer >= 0)
        {
            MoveHorizontal(1, 1, 10);
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) Jump(1000);
    }

    public void MoveHorizontal (float speed, int direction, float time)
    {
        //direction = 1 means that it'll move right direction = -1 means it'll move left
        //setTimer(time);
        Vector3 _speed = new Vector3(speed, 0, 0);
        Debug.Log(Time.deltaTime);
        this.transform.position += new Vector3(speed * direction / 100,0 , 0);
    }

    public void Jump(float force)
    {
        rb.AddForce(new Vector3(0, force, 0));
    }

    public void DestroyObject(string tag)
    {
        foreach (GameObject go in goOnCollision)
        {
            if (go.tag == tag)
            {
                Destroy(go);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check what gameObjects we collide with so we can destroy them when we are on range.
        goOnCollision.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        goOnCollision.Remove(collision.gameObject);
    }
    
    public void setTimer(float time)
    {
        timer = time;
    }
}
