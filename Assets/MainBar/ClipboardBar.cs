using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardBar : MonoBehaviour {

    private string before;
    private string playmode;

    public string getBefore()
    {
        return before;
    }

    public void setBefore(string newBefore)
    {
        before = newBefore;
    }

    public string getPlaymode()
    {
        return playmode;
    }

    public void setPlaymode(string newPlaymode)
    {
        playmode = newPlaymode;
    }

}
