using UnityEngine;
using System.Collections;

public class LifeCycle : MonoBehaviour {
    
    void Awake()
    {
        EventBus.Register(this);
        Application.LoadLevelAdditive("SoftwareCityScene");
        Application.LoadLevelAdditive("EnvironmentScene");
        if (UnityEngine.VR.VRDevice.isPresent)
        {
            Application.LoadLevelAdditive("PlayerScene");
        } else
        {
            Application.LoadLevelAdditive("PlayerSceneFallback");

            Invoke("PostSceneReady", 1);
        }
        
    }

    private void PostSceneReady()
    {
        EventBus.Post(new SceneReadyEvent());
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