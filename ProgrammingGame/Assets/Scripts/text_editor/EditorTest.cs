using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorTest : MonoBehaviour
{
    #region Public Attributes

    public InputField textEditorInputField;
    public Text textEditorLinesText;
    public Text uglyText;
    public Text enrichedText;
    public RectTransform content;
    public Scrollbar scrollbar;

    private string defaultPrototypeUgly = @"import(""array"")

function pop(self)
    return array.pop(self.arr)
    end

function push(self, element)
    return array.append(self.arr, element)
    end

function new_stack(arr) 
    return { arr=arr, pop=pop, push=push }
end

    stack = new_stack({})

print(stack.push(1))
print(stack.push(2))
print(stack.push(3))

print(stack.pop())
";
    private string defaultPrototype = @"import(""array"")

function pop(self)
    return array.pop(self.arr)
    end

function push(self, element)
    return array.append(self.arr, element)
    end

function new_stack(arr) 
    return { arr=arr, pop=pop, push=push }
end

    stack = new_stack({})

print(stack.push(1))
print(stack.push(2))
print(stack.push(3))

print(stack.pop())
";

    #endregion

    #region Private Attributes

    private int _nLines = 0;
    private string _enrichedContents;
    private string _uglyContents;

    #endregion

    #region Properties

    public int LineCount
    {
        get { return _nLines; }//inputField.text.Split('\n').Length; }
        private set { _nLines = value; }
    }

    public string Code
    {
        get => textEditorInputField.text;
    }

    public string UglyContents
    {
        get { return textEditorInputField.text; }
        private set { textEditorInputField.text = value; }
    }

    public string EnrichedContents
    {
        get { return enrichedText.text; }
        private set { enrichedText.text = value; }
    }

    #endregion

    #region Monobehavior

    public void Awake()
    {
        _uglyContents = defaultPrototypeUgly;
        _enrichedContents = defaultPrototype;
    }

    public void Start()
    {
        UglyContents = _uglyContents;
        EnrichedContents = _enrichedContents;
        //RectTransform t = uglyText.GetComponent<RectTransform>();

        textEditorInputField.onValueChanged.AddListener((s) =>
        {
            //content;
            int caretPosition = textEditorInputField.caretPosition;
            TextGenerator gen = uglyText.cachedTextGenerator;
            Debug.Log(caretPosition);
            Debug.Log(gen.characters.Count);
            //UICharInfo charInfo = gen.characters[caretPosition - 1];
            bool isLast = caretPosition >= gen.characters.Count;// - 1;
            //float caretHeight = (charInfo.cursorPos.y) / textEditorText.pixelsPerUnit;

            if (isLast)
            {
                StartCoroutine(MoveTextEnd_NextFrame(s));
            }

            EnrichedContents = ToRichText(UglyContents);
        });
    }
    
    IEnumerator MoveTextEnd_NextFrame(string s)
    {
        yield return new WaitForSeconds(0.1f); // Skip the first frame in which this is called.
        float offset = 740f;
        float min = Mathf.Max(0f, uglyText.preferredHeight - offset);
        content.anchoredPosition = new Vector2(0f, min);
    }

    public void Update()
    {
        // check for number of lines
        LineCount = CheckNumberOfLines();
        RectTransform rect = textEditorInputField.GetComponent<RectTransform>();
        Text textRect = textEditorInputField.textComponent;

        float height = Mathf.Max(textRect.preferredHeight + 40, 800.0f);
        //float enrHeight = Mathf.Max(enrichedText.preferredHeight + 20, 780.0f);

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);

        content.sizeDelta = new Vector2(content.sizeDelta.x, height);

        RectTransform linesRect = textEditorLinesText.GetComponent<RectTransform>();
        linesRect.sizeDelta = new Vector2(linesRect.sizeDelta.x, height);

        //RectTransform enrTextRectTransform = enrichedText.GetComponent<RectTransform>();
        //enrTextRectTransform.sizeDelta = new Vector2(enrTextRectTransform.sizeDelta.x, height);

        //content.anchoredPosition = new Vector2(0f, );
    }

    private int CheckNumberOfLines()
    {
        int nLines = UglyContents.Split('\n').Length;
        LinesToTextLines(nLines);
        return nLines;
    }

    private void LinesToTextLines(int nLines)
    {
        string linesStr = "";
        for (int i = 1; i <= nLines; i++)
        {
            linesStr += i.ToString() + "\n";
        }
        textEditorLinesText.text = linesStr;
    }

    private string ToRichText(string code)
    {
        string str = "";

        str += "<color=red>" + code + "</color>";

        return str;
    }

    #endregion
}
