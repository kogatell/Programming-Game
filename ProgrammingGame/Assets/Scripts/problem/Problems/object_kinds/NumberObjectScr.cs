using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectContainer", menuName = "ObjectContainer/Number", order = 2)]
public class NumberObjectScr : ObjectContainer
{
    private double value = 0.0;
    
    public override Object GetObject()
    {
        return new Number(value);
    }
}
