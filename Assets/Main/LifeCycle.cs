using UnityEngine;
using System.Collections;

public class LifeCycle : MonoBehaviour {

    public delegate void OnBegin();
    public delegate void OnFinish();

    public OnBegin OnBeginHandler;
    public OnFinish OnFinishHandler;

    void Awake()
    {
        Application.LoadLevelAdditive("SoftwareCityScene");
    }

    void Start()
    {
        Begin();
    }

    public void Begin()
    {
        OnBeginHandler();
    }

    public void Finish()
    {
        OnFinishHandler();
    }


    // Update is called once per frame
    void Update () {
	
	}
}
