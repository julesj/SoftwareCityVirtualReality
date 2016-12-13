using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Hint : MonoBehaviour {

    private static Dictionary<string, HintState> allHints = new Dictionary<string, HintState>();
    internal class HintState
    {
        internal string name;
        internal Hint hint;
        internal int displayedCount;
        internal int maxDisplayCount;
        internal bool visible = false;
    }

    public static void Display(string name)
    {
        if (!allHints.ContainsKey(name))
        {
            Debug.Log("Unable to display hint " + name + ": Not registered");
            return;
        }
        HintState state = allHints[name];
        if (!state.visible)
        {
            if (state.displayedCount < state.maxDisplayCount)
            {
                state.hint.transform.Find("Body").gameObject.SetActive(true);
                state.hint.transform.Find("Body").GetComponent<AnimateThis>().Transformate()
                .FromScale(0)
                .ToScale(1)
                .Duration(1)
                .Ease(AnimateThis.EaseOutElastic)
                .Start();
            }
            state.displayedCount++;
            state.visible = true;
        }
    }

    public static void Hide(string name)
    {
        if (!allHints.ContainsKey(name))
        {
            Debug.Log("Unable to hide hint " + name + ": Not registered");
            return;
        }
        HintState state = allHints[name];
        state.visible = false;
        /*state.hint.transform.Find("Body").GetComponent<AnimateThis>().Transformate()
            .FromScale(1)
            .ToScale(0)
            .Duration(0.125f)
            .Start();
        // FIXME: End Action*/
        state.hint.transform.Find("Body").gameObject.SetActive(false);
        state.hint.transform.Find("Body").transform.localScale = new Vector3(0, 0, 0);
    }

    public static void Confirm(string name)
    {
        if (!allHints.ContainsKey(name))
        {
            Debug.Log("Unable to confirm hint " + name + ": Not registered");
            return;
        }
        HintState state = allHints[name];
        state.displayedCount = state.maxDisplayCount;
        Hide(name);
    }

    public static void Reset()
    {
        foreach (HintState state in allHints.Values)
        {
            state.displayedCount = 0;
        }
    }

    public string name;
    public int maxDisplayCount = 1;
    [Multiline]
    public string text;

	void Start () {
        if (name == null || name.Length == 0)
        {
            name = transform.name;
        }
	    if (!allHints.ContainsKey(name))
        {
            HintState state = new HintState();
            state.displayedCount = 0;
            state.hint = this;
            state.maxDisplayCount = maxDisplayCount;
            state.name = name;
            allHints.Add(name, state);
        }
        GetComponentInChildren<TextMesh>().text = text;
        transform.Find("Body").transform.localScale = new Vector3(0, 0, 0);
        transform.Find("Body").gameObject.SetActive(false);
	}
}
