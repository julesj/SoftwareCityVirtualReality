using UnityEngine;
using System.Collections;

public class BoinzBoinz : MonoBehaviour {

    private Vector3 startOffset;
    public float boinzHeight = 0.25f;
    public float boinzSpeed = 1;

    void Start()
    {
        startOffset = transform.localPosition;
    }
	// Update is called once per frame
	void Update () {
        transform.localPosition = startOffset + new Vector3(0, Mathf.Abs(Mathf.Sin(Time.time * Mathf.PI * boinzSpeed)) * boinzHeight, 0);
        transform.localRotation = Quaternion.AxisAngle(Vector3.up, Time.time);
	}
}
