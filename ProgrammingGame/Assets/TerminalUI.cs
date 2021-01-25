using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TerminalUI : MonoBehaviour
{
	#region Private Variables

	private ScrollRect terminal;
	private float offset;
	
	[SerializeField]
	private Text text;

	[SerializeField] private RectTransform viewport;

	[SerializeField] private InputField input;

	private Scrollbar scroll;
	#endregion

	#region Public Variables

	#endregion

	#region Properties

	#endregion

	#region MonoBehaviour

    private void Awake()
    {
	    terminal = GetComponent<ScrollRect>();
	    Rect rect = GetComponent<RectTransform>().rect;
	    viewport.sizeDelta = new Vector2(viewport.sizeDelta.x, rect.height);
	    input.onValueChanged.AddListener((s) =>
	    {
		    if (s.Substring(0, Math.Min(s.Length, 3)) != ">> ")
		    {
			    input.text = ">> ";
			    input.ActivateInputField();
			    input.Select();
			    StartCoroutine(MoveTextEnd_NextFrame());
		    }
	    });
	    input.text = ">> ";
	    
    }

    private void Update()
    {
	    
    }

    #endregion

    #region Public Methods

    public void Write(string code)
    {
	    
	    string[] lines = code.Split('\n');
	    foreach (string line in lines)
	    {
		    text.text += "\n";
		    text.text += line;
		    offset = text.preferredHeight;
		    viewport.sizeDelta = new Vector2(viewport.sizeDelta.x, (text.preferredHeight + 20));
	    }
	    terminal.content.anchoredPosition = new Vector2(0f, offset - 170);
    }

    public void AddListener(UnityAction<string> action)
    {
	    input.onEndEdit.AddListener((s) =>
	    {
		    if (s.TrimEnd() == ">>")
		    {
			    return;
		    }
		    action(s);
		    input.text = ">> ";
		    input.ActivateInputField();
		    input.Select();
		    StartCoroutine(MoveTextEnd_NextFrame());
	    });
    }

    IEnumerator MoveTextEnd_NextFrame()
    {
	    yield return 0; // Skip the first frame in which this is called.
	    input.MoveTextEnd(false); // Do this during the next frame.
    }

    
    public void OnSelect()
    {
	    input.ActivateInputField();
	    input.Select();
	    StartCoroutine(MoveTextEnd_NextFrame());  
    }
    #endregion

    #region Private Methods
	
    #endregion
}
