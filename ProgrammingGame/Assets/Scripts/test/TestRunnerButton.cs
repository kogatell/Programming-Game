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
        
       /* FindObjectOfType<Runner>().ExecuteViaProblem(@"                
            function new()                
                return { sum=add, value=0 }
            end
            
            function add(this, arr)                
                sum = 0
                for i=0, #arr do -- #arr gets length of the array
                    sum = sum + arr[i]
                end
                this.value = sum + this.value
                return sum
            end
            
            return new
        ", true);*/

       FindObjectOfType<Runner>().ExecuteViaProblem(@"                            
            
            function returner(random_input)                
                return random_input
            end
            
            return returner
        ", true);
    }
    
}
