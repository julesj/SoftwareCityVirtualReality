using UnityEngine;
using System.Collections;

public class LifeCycle : MonoBehaviour {

    public delegate void OnBegin();
    public delegate void OnFinish();

    public OnBegin OnBeginHandler;
    public OnFinish OnFinishHandler;

    void Awake()
    {
        Application.LoadLevelAdditive("PlayerScene");
        Application.LoadLevelAdditive("SoftwareCityScene");
        Application.LoadLevelAdditive("EnvironmentScene");
    }

    void Start()
    {
        Begin();
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

}
