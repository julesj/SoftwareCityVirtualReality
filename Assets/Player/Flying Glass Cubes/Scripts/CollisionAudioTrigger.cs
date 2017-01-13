using UnityEngine;
using System.Collections;

public class CollisionAudioTrigger : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        GetComponent<AudioPlayer>().Play();
    }
}
