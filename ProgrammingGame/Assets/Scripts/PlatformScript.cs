using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    private GameObject[] sons;
    private GameObject[] text;

    public GameObject[] Sons => sons;

    // Start is called before the first frame update
    void Start()
    {
        int childCount = transform.childCount;
        sons = new GameObject[childCount];
        text = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            
            sons[i] = this.transform.GetChild(i).gameObject;
            
            text[i] = sons[i].transform.GetChild(0).gameObject;
            if (text[i].CompareTag("Text"))
            {
                text[i].GetComponent<TextMesh>().text = i.ToString();
            }
        }
        float distance = Vector3.Distance(this.transform.GetChild(0).transform.position, this.transform.GetChild(1).transform.position);//Give max distance between two platforms
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        manager.GetComponent<VariableScript>().maxdistance = distance + distance / 2;
        Debug.Log(manager.GetComponent<VariableScript>().maxdistance);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
