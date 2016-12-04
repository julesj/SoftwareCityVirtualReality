﻿using UnityEngine;
using System.Collections;

public class LifeCycle : MonoBehaviour {
    
    void Awake()
    {
        EventBus.Register(this);
        Application.LoadLevelAdditive("PlayerSceneFallback");
        Application.LoadLevelAdditive("SoftwareCityScene");
        Application.LoadLevelAdditive("EnvironmentScene");
        Invoke("SceneReady", 2);
    }


    private void SceneReady()
    {
        EventBus.Post(new SceneReadyEvent());
    }

    public void Restart()
    {
        Application.LoadLevel("MainScene");
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