using UnityEngine;
using System.Collections;

public class LifeCycle : MonoBehaviour {

    public delegate void OnInit();
    public delegate void OnShutdown();
    public OnInit OnInitHandler;
    public OnShutdown OnShutdownHandler;

    public delegate void OnBegin();
    public delegate void OnFinish();
    public OnInit OnBeginHandler;
    public OnShutdown OnFinishHandler;


    void Awake()
    {
        Application.LoadLevelAdditive("PlayerScene");
        Application.LoadLevelAdditive("SoftwareCityScene");
        Application.LoadLevelAdditive("EnvironmentScene");
        Invoke("Init", 1);
    }


    public void Init()
    {
        if (OnInitHandler != null)
        {
            OnInitHandler();
        }
    }

    public void Shutdown()
    {
        if (OnShutdownHandler != null)
        {
            OnShutdownHandler();
        }
    }

    public void Begin()
    {
        if (OnBeginHandler != null)
        {
            OnBeginHandler();
        }
    }

    public void Finish()
    {
        if (OnFinishHandler != null)
        {
            OnFinishHandler();
        }
    }

    public void Restart()
    {
        Application.LoadLevel("MainScene");
    }

}
