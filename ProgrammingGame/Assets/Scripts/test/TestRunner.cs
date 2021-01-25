using UnityEngine;
using UnityEngine.UI;

public class TestRunner : MonoBehaviour
{
    private Runner runner;
    private Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        runner = FindObjectOfType<Runner>();
        text = GetComponent<Text>();
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
        text.text = $"State: {runner.state}. Last returned: {runner.LastReturnedObject}";    
        
        
    }
    
}
