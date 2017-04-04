using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardBar : MonoBehaviour {

    private string before;
    private string playmode;

    public enum LoadableScenes
    {
        MainSceneBar,
        Controller_WelcomeScene,
        Gesten_WelcomeScene,
        Mixed_WelcomeScene,
        PlayerSceneContrBar,
        PlayerSceneGestureBar,
        PlayerSceneMixedBar,
        ScaleRotateExampleScene
    };

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
