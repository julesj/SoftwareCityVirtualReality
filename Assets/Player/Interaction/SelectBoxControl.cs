using UnityEngine;
using System.Collections;

public class SelectBoxControl : MonoBehaviour {

    public delegate void OnSelectBoxChangedAction(string value);
    public OnSelectBoxChangedAction OnSelectBoxChanged;

    public SliderControl sliderControl;
    public TextMesh linesBefore;
    public TextMesh selectedLines;
    public TextMesh linesAfter;
    public int numLinesBefore = 3;
    public int numLinesAfter = 3;

    private int lastIndex = -1;

    private string[] _model = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K"};

    public string[] model
    {
        get
        {
            return _model;
        }
        set
        {
            _model = value;
            lastIndex = -1;
        }
    }

    void Start() {
        sliderControl.OnValueChanged = SetValue;
        SetValue(0);
	}

    private void SetValue(float value)
    {
        int i = (int)(value * (model.Length-1));

        if (i == lastIndex)
        {
            return;
        }
        lastIndex = i;
        
        linesBefore.text = "";
        for (int j = Mathf.Max(0, i-numLinesBefore); j< i; j++)
        {
            if (linesBefore.text.Length != 0)
            {
                linesBefore.text += "\n";
            }
            linesBefore.text += model[j];
        }
        selectedLines.text = model[i];
        linesAfter.text = "";
        for (int j = i + 1; j <= Mathf.Min(model.Length - 1, i + numLinesAfter); j++)
        {
            if (linesAfter.text.Length != 0)
            {
                linesAfter.text += "\n";
            }
            linesAfter.text += model[j];
        }

        if (OnSelectBoxChanged != null)
        {
            OnSelectBoxChanged(model[i]);
        }
    }
}
