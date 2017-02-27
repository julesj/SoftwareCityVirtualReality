using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardBar : MonoBehaviour {

    private string before;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public string getBefore()
    {
        return before;
    }

    public void setBefore(string newBefore)
    {
        before = newBefore;
    }

}
