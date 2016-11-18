using UnityEngine;
using System.Collections;

public class DestroyOnBegin : MonoBehaviour {

    void Awake()
    {
        EventBus.Register(this);
    }

    public void OnEvent(StartPlayingEvent e)
    {
        EventBus.Unregister(this);
        GameObject.Destroy(gameObject);
    }
}
