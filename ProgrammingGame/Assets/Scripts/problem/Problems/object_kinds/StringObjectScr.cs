using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectContainer", menuName = "ObjectContainer/String", order = 2)]
public class StringObjectScr : ObjectContainer
{
    [SerializeField]
    private string value;

    public override Object GetObject()
    {
        return new String(value);
    }
}
