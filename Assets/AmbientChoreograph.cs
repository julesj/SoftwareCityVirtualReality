using UnityEngine;
using System.Collections;

public class AmbientChoreograph : MonoBehaviour {

    private AudioSource ambient1;
    private AudioSource ambient2;
    private AudioSource welcome;
    private AudioSource goodbye;

    void Awake()
    {
        ambient1 = GetComponents<AudioSource>()[0];
        ambient2 = GetComponents<AudioSource>()[1];
        welcome = GetComponents<AudioSource>()[2];
        goodbye = GetComponents<AudioSource>()[3];

        LifeCycle lifeCycle = FindObjectOfType<LifeCycle>();
        if (lifeCycle != null)
        {
            lifeCycle.OnBeginHandler += OnBegin;
            lifeCycle.OnFinishHandler += OnFinish;
        }
    }

    private void OnBegin()
    {
        // todo Fading
        ambient2.volume = 1;
        welcome.Play();
    }


    private void OnFinish()
    {
        // todo Fading
        ambient2.volume = 0;
        goodbye.Play();
    }
}
