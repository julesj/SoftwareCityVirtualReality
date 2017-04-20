using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PositionCity : MonoBehaviour {
    Transform headsetTransform;
    bool positioned = false;

	void Update () {
		if (!headsetTransform || headsetTransform.position.x == 0)
        {
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
        }

        if (headsetTransform && !positioned)
        {
            gameObject.transform.position = new Vector3(headsetTransform.position.x + headsetTransform.forward.x, 0, headsetTransform.position.z + headsetTransform.forward.z);
            positioned = true;
        }
	}
}
