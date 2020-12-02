using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorTest : MonoBehaviour
{
    #region Attributes

    public InputField inputField;

    #endregion

    #region Monobehavior

    public void Start()
    {
        if (inputField.lineType == InputField.LineType.SingleLine)
        {
            inputField.lineType = InputField.LineType.MultiLineNewline;
        }
    }

    #endregion
}
