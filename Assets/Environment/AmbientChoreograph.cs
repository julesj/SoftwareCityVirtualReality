using UnityEngine;
using System.Collections;

public class AmbientChoreograph : MonoBehaviour {

    private AudioSource ambient1;
    private AudioSource ambient2;
    private AudioSource welcome;
    private AudioSource goodbye;

    public void Awake()
    {
        EventBus.Register(this);
        ambient1 = GetComponents<AudioSource>()[0];
        ambient2 = GetComponents<AudioSource>()[1];
        welcome = GetComponents<AudioSource>()[2];
        goodbye = GetComponents<AudioSource>()[3];
    }

    public void OnEvent(StartPlayingEvent e)
    {
        // todo Fading
        ambient2.volume = 1;
        welcome.Play();
    }


    public void OnEvent(StopPlayingEvent e)
    {
        // todo Fading
        ambient2.volume = 0;
        goodbye.Play();
    }
}
