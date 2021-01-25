using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public bool isQuit;
    public string nameOfScene;
    private TextMesh text;
    private bool insideText;
    void Start()
    {
        text = this.GetComponent<TextMesh>();
        text.color = Color.white;
        insideText = false;
    }

    // Update is called once per frame
    private void OnMouseEnter()
    {
        text.color = Color.blue;
        insideText = true;

    }

    private void OnMouseExit()
    {
        text.color = Color.white;
        insideText = false;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && insideText)
        {
            if (!isQuit)
            {
                SceneManager.LoadScene(1);
            }
            else Application.Quit();
                
        }
    }
}
