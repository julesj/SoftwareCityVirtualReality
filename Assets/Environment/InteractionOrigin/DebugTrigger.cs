using UnityEngine;
using System.Collections;

public class DebugTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponentInChildren<Camera>() != null)
        {
            FindObjectOfType<LifeCycle>().Begin();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponentInChildren<Camera>() != null)
        {
            FindObjectOfType<LifeCycle>().Finish();
        }
    }

}
