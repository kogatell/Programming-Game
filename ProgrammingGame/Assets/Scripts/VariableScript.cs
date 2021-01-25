using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableScript : MonoBehaviour
{
    public List<Transform> points;
    public float maxdistance;
    void Awake()
    {
        //StartCoroutine(Do());
        Debug.Log("Im doing it");
        GameObject levelObject = GameObject.FindGameObjectWithTag("Level");
        for (int i = 0; i < levelObject.transform.childCount; i++)//Get all children
        {
            GameObject child = levelObject.transform.GetChild(i).gameObject;
            for (int j = 0; j < child.transform.childCount; j++)
            {
                GameObject child2 = child.transform.GetChild(j).gameObject;
                if (child2.CompareTag("Location"))
                {
                    points.Add(child2.transform);
                }

            }
        }
        
    }
    /*IEnumerator Do()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Im doing it");
        GameObject levelObject = GameObject.FindGameObjectWithTag("Level");
        for (int i = 0; i < levelObject.transform.childCount; i++)//Get all children
        {
            GameObject child = levelObject.transform.GetChild(i).gameObject;
            for (int j = 0; j < child.transform.childCount; j++)
            {
                GameObject child2 = child.transform.GetChild(j).gameObject;
                if (child2.CompareTag("Location"))
                {
                    points.Add(child2.transform);
                }

            }
        }
    }*/
}
