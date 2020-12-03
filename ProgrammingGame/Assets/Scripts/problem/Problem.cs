using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Problem", menuName = "Problems/ProblemObject", order = 1)]
public class Problem : ScriptableObject
{
    public string Name;
    public Lib[] Libs;
    [FormerlySerializedAs("cases")] public TestCase[] Cases;
}
