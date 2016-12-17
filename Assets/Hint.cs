using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Hint : MonoBehaviour {

    private enum DisplayState
    {
        Invisible, WaitingForDisplay, Displaying
    }

    public class DisplayHintEvent
    {
        public string name;
        public DisplayHintEvent(string name)
        {
            this.name = name;
        }
    }

    public class HideHintEvent
    {
        public string name;
        public HideHintEvent(string name)
        {
            this.name = name;
        }
    }

    public class ConfirmHintEvent
    {
        public string name;
        public ConfirmHintEvent(string name)
        {
            this.name = name;
        }
    }

    public class ResetAllHintsEvent
    {
    }

    public static void Display(string name)
    {
        EventBus.Post(new DisplayHintEvent(name));
    }

    public static void Hide(string name)
    {
        EventBus.Post(new HideHintEvent(name));
    }

    public static void Confirm(string name)
    {
        EventBus.Post(new ConfirmHintEvent(name));
    }

    public static void Reset()
    {
        EventBus.Post(new ResetAllHintsEvent());
    }

    public string name;
    public string dependsOn;
    public int maxDisplayCount = 1;
    public float initialDelay = 0.5f;
    public float delay = 0.5f;
    [Multiline]
    public string text;

    private int displayedCount;
    private DisplayState displayState = DisplayState.Invisible;
    private bool confirmed;
    private float waitUntil;
    private HashSet<string> confirmedHints = new HashSet<string>();
    private HashSet<string> preconditionHints = new HashSet<string>();

    void Awake()
    {
        EventBus.Register(this);
    }

	void Start () {
        if (name == null || name.Length == 0)
        {
            name = transform.name;
        }      

        GetComponentInChildren<TextMesh>().text = text;
        transform.Find("Body").transform.localScale = new Vector3(0, 0, 0);
        transform.Find("Body").gameObject.SetActive(false);
        foreach (string precondition in dependsOn.Split(' '))
        {
            if (precondition.Length > 0)
            {
                preconditionHints.Add(precondition);
            }
        }
	}

    private bool DependenciesSatisfied()
    {
        return preconditionHints.Count == 0 || preconditionHints.IsSubsetOf(confirmedHints);
    }

    public void OnEvent(ResetAllHintsEvent e)
    {
        displayedCount = 0;
        confirmed = false;
        confirmedHints.Clear();
        HideMe();
    }

    public void OnEvent(ConfirmHintEvent e)
    {
        confirmedHints.Add(e.name);
        if (e.name.Equals(name))
        {
            confirmed = true;
            HideMe();
        }
    }

    public void OnEvent(HideHintEvent e)
    {
        if (e.name.Equals(name) && (displayState != DisplayState.Invisible))
        {
            
            /*transform.Find("Body").GetComponent<AnimateThis>().Transformate()
                .FromScale(1)
                .ToScale(0)
                .Duration(0.125f)
                .OnEnd(disableMe)
                .Start();*/
            HideMe();
        }
    }

    public void OnEvent(DisplayHintEvent e)
    {
        if (e.name.Equals(name) && displayState == DisplayState.Invisible && !confirmed && DependenciesSatisfied())
        {
            displayState = DisplayState.WaitingForDisplay;
            Invoke("ShowMe", displayedCount == 0 ? initialDelay : delay);
        }
    }

    private void HideMe()
    {
        transform.Find("Body").gameObject.SetActive(false);
        transform.Find("Body").transform.localScale = new Vector3(0, 0, 0);
        CancelInvoke();
        displayState = DisplayState.Invisible;
    }

    private void ShowMe()
    {
        if (displayState != DisplayState.Displaying)
        {
            if (displayedCount < maxDisplayCount)
            {
                transform.Find("Body").gameObject.SetActive(true);
                transform.Find("Body").GetComponent<AnimateThis>().Transformate()
                .FromScale(0)
                .ToScale(1)
                .Duration(1)
                .Ease(AnimateThis.EaseOutElastic)
                .Start();
            }
            displayedCount++;
            displayState = DisplayState.Displaying;
        }
    }
}
