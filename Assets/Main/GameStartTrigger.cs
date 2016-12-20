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
        if (GetNumberOfColliders<GameStarter>() == 2)
        {
            FindObjectOfType<LifeCycle>().GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.StopPlaying);
        }

        // reset stuff when HMD is in triggerSphere
        if (other.GetComponent<GameReseter>() != null)
        {
            EventBus.Post(new Events.ResetPlayerEvent());
            EventBus.Post(new Events.ClearDisplayEvent());
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // start playing when no controller is inside the triggerSphere
        if (GetNumberOfColliders<GameStarter>() == 0)
        {
            FindObjectOfType<LifeCycle>().GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.StartPlaying);
        }
    }

    private int GetNumberOfColliders<T>() 
    {
        int counter = 0;

        // get all colliders in range of triggerSphere
        Collider[] colliders = Physics.OverlapSphere(triggerSphere.transform.position, triggerSphere.radius);
        foreach (Collider collider in colliders)
        {
            // increase counter if collider is from correnct type
            if (collider.gameObject.GetComponent<T>() != null)
            {
                counter++;
            }
        }

        return counter;
    }

}
