using UnityEngine;
using System.Collections;

public class GameStarter : MonoBehaviour {
    private static int counter = 0;

    public void Start()
    {
        counter = 0;
    }

    public void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.GetComponent<GameStartTrigger>() != null)
        {
            counter++;
        }
        if (counter == 2)
        {
            FindObjectOfType<LifeCycle>().GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.StopPlaying);
        }

        Debug.Log(gameObject + " entered, counter = " + counter);
    }

    public void OnTriggerExit(Collider trigger)
    {
        if (trigger.gameObject.GetComponent<GameStartTrigger>() != null)
        {
            counter--;
        }
        if (counter == 0)
        {
            FindObjectOfType<LifeCycle>().GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.StartPlaying);
        }

        Debug.Log(gameObject + " entered, counter = " + counter);
    }

}
