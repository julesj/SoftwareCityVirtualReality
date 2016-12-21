using UnityEngine;
using System.Collections;
using VRTK;

public class BoinzBoinz : MonoBehaviour {

    private Vector3 startOffset;

    public bool hapticLeft = false;
    public bool hapticRight = false;
    public float hapticStrength = 0.1f;

    public float boinzHeight = 0.25f;
    public float boinzSpeed = 1;

    private float lastBoinzHeight;

    void Start()
    {
        startOffset = transform.localPosition;
    }
	// Update is called once per frame
	void Update () {
        float height = Mathf.Sin(Time.time * Mathf.PI * boinzSpeed) * boinzHeight;

        if (Mathf.Sign(height) != Mathf.Sign(lastBoinzHeight))
        {
            if (hapticLeft)
            {
                VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerActions>().TriggerHapticPulse((ushort)(hapticStrength * 3999));
            }
            if (hapticRight)
            {
                VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerActions>().TriggerHapticPulse((ushort)(hapticStrength * 3999));
            }
        }
        lastBoinzHeight = height;

        transform.localPosition = startOffset + new Vector3(0, Mathf.Abs(height), 0);
        transform.localRotation = Quaternion.AxisAngle(Vector3.up, Time.time);

	}
}
