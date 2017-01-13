using UnityEngine;
using System.Collections;
using VRTK;

public class ControlTrigger : MonoBehaviour {
    public bool IsPressed()
    {
        return VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>().triggerPressed; //fixme
    }
}
