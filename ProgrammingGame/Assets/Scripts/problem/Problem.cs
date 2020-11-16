using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Problem", menuName = "Problems/ProblemObject", order = 1)]
public class Problem : ScriptableObject
{
    public string Name;
    public Lib[] Libs;
}
