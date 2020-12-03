using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestCase", menuName = "TestCases/TestCase", order = 2)]
public class TestCase : ScriptableObject
{
    [SerializeField]
    private string jsonData;

    public string JsonData => jsonData;

    [SerializeField]
    private string inputData;

    /// <summary>
    /// Input for the function (each element is a parameter)
    /// </summary>
    public Object[] InputData => (Object.FromJsonString(inputData) as ArrayObject).array.ToArray();

    /// <summary>
    /// Tests if the test case passes
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public bool Test(Object input)
    {
        Object data = Object.FromJsonString(JsonData);
        return data.EqualDeep(input);
    }
    
}
