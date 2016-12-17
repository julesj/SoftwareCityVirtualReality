using UnityEngine;
using System.Collections;

public class LifeCycle : MonoBehaviour {
    
    void Awake()
    {
        EventBus.Register(this);
        Application.LoadLevelAdditive("SoftwareCityScene");
        Application.LoadLevelAdditive("EnvironmentScene");
        Application.LoadLevelAdditive("PlayerScene");
    }

    public void Restart()
    {
        Application.LoadLevel("MainScene");
    }

    public void OnEvent(StopPlayingEvent e)
    {
        Hint.Reset();
    }

}

public class SceneReadyEvent
{
}

public class StartPlayingEvent
{
}

public class StopPlayingEvent
{
}