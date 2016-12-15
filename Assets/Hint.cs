using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Hint : MonoBehaviour {

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
    public int maxDisplayCount = 1;
    [Multiline]
    public string text;

    private int displayedCount;
    private bool visible;
    private bool confirmed;

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
	}

    public void OnEvent(ResetAllHintsEvent e)
    {
        displayedCount = 0;
        confirmed = false;
        visible = false;
        transform.Find("Body").gameObject.SetActive(false);
        transform.Find("Body").transform.localScale = new Vector3(0, 0, 0);
    }

    public void OnEvent(ConfirmHintEvent e)
    {
        if (e.name.Equals(name))
        {
            confirmed = true;
            Hide(name);
        }
    }

    public void OnEvent(HideHintEvent e)
    {
        if (e.name.Equals(name) && visible)
        {
            visible = false;
            /*transform.Find("Body").GetComponent<AnimateThis>().Transformate()
                .FromScale(1)
                .ToScale(0)
                .Duration(0.125f)
                .OnEnd(disableMe)
                .Start();*/
            transform.Find("Body").gameObject.SetActive(false);
            transform.Find("Body").transform.localScale = new Vector3(0, 0, 0);

        }
    }

    private void disableMe()
    {
        transform.Find("Body").gameObject.SetActive(false);
        transform.Find("Body").transform.localScale = new Vector3(0, 0, 0);
    }

    public void OnEvent(DisplayHintEvent e)
    {
        if (e.name.Equals(name) &&  !visible && !confirmed)
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
            visible = true;
        }
    }
}
