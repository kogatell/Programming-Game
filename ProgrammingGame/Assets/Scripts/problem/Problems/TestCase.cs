using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestCase", menuName = "TestCases/TestCase", order = 2)]
public class TestCase : ScriptableObject
{

    /// <summary>
    /// If the thing that returns the function should be go through a pipeline
    /// </summary>
    [SerializeField] private bool isInstance = false;

    /// <summary>
    /// Only if isInstance = true
    ///
    /// the pipeline that will be executed on the instance
    ///
    /// { "data": [[name, [..params.. (if null, means that it's not function)], expectedReturnValue] ...]  }  
    /// </summary>
    [TextArea(15,20)]
    [SerializeField] private string pipeline;
    
    [TextArea(15,20)]
    [SerializeField]
    private string jsonData;

    public string JsonData => jsonData;

    [TextArea(15,20)]
    [SerializeField]
    private string inputData;

    /// <summary>
    /// Input for the function (each element is a parameter)
    /// </summary>
    public Object[] InputData => (Object.FromJsonString(inputData) as ArrayObject).array.ToArray();

    public Object[] Pipeline => (Object.FromJsonString(pipeline) as ArrayObject).array.ToArray();

    public bool IsInstance => isInstance;

    /// <summary>
    /// Tests if the test case passes
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public bool Test(Object input)
    {
        Object data = Object.FromJsonString(JsonData);
        if (data == Null.NULL) return true;
        return data.EqualDeep(input);
    }
    
}
