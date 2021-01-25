using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Lib : MonoBehaviour
{
    public abstract Context InjectLibrary(Context context);
    [SerializeField]
    private Doc[] documentation;
    [SerializeField] private string libName;
    public Doc[] Documentation => documentation;

    public string LibName => libName;
}

