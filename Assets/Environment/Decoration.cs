using UnityEngine;
using System.Collections;

public class Decoration : MonoBehaviour {

    public float tractorBeamSpeed = 0.01f;

    public void TractorBeamToPosition(Vector3 position)
    {
        Vector3 velocity = (position - transform.position).normalized;
        gameObject.GetComponent<Rigidbody>().AddForce((velocity * tractorBeamSpeed) * (Vector3.Distance(position, transform.position)), ForceMode.Impulse);
    }
}