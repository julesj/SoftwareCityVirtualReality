using UnityEngine;
using System.Collections;

public class GameStartTrigger : MonoBehaviour {

    private SphereCollider triggerSphere;

    public void Awake()
    {
        triggerSphere = gameObject.GetComponent<SphereCollider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        // stop playing when both controllers are inside the triggerSphere
        if (GetNumberOfGamestarter() == 2)
        {
            FindObjectOfType<LifeCycle>().GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.StopPlaying);
        }

        Debug.Log(gameObject + " entered, counter = " + GetNumberOfGamestarter());
    }

    public void OnTriggerExit(Collider other)
    {
        // start playing when no controller is inside the triggerSphere
        if (GetNumberOfGamestarter() == 0)
        {
            FindObjectOfType<LifeCycle>().GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.StartPlaying);
        }

        Debug.Log(gameObject + " entered, counter = " + GetNumberOfGamestarter());
    }

    private int GetNumberOfGamestarter()
    {
        int numberOfGameStarter = 0;

        // get all colliders in range of triggerSphere
        Collider[] colliders = Physics.OverlapSphere(triggerSphere.transform.position, triggerSphere.radius);

        foreach (Collider collider in colliders)
        {
            // increase counter if collider is a GameStarter
            if (collider.gameObject.GetComponent<GameStarter>() != null)
            {
                numberOfGameStarter++;
            }
        }

        return numberOfGameStarter;
    } 

}
