using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRunnerButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        
        FindObjectOfType<Runner>().ExecuteViaProblem(@"                
            function test(parameters)
                print(parameters)
                return parameters
            end
            return test
        ", true);
    }
    
}
