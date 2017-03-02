using UnityEngine;
using System.Collections;
using VRTK;

public class DeviceMonitor : MonoBehaviour {

    ControllerInteractionEventHandler controllerEnabledHandler;
    ControllerInteractionEventHandler controllerDisabledHandler;

    public void Awake () {
        CheckDevices();

        controllerEnabledHandler = new ControllerInteractionEventHandler(ControllerEnabled);
        controllerDisabledHandler = new ControllerInteractionEventHandler(ControllerDisabled);
    }

    public void Start ()
    {
        GameObject leftController = VRTK_DeviceFinder.GetControllerLeftHand();
        leftController.GetComponent<VRTK_ControllerEvents>().ControllerEnabled += controllerEnabledHandler;

        GameObject rightController = VRTK_DeviceFinder.GetControllerRightHand();
        rightController.GetComponent<VRTK_ControllerEvents>().ControllerDisabled += controllerDisabledHandler;
    }
	
	private void CheckDevices()
    {
        GameObject leftController = VRTK_DeviceFinder.GetControllerLeftHand();
        GameObject rightController = VRTK_DeviceFinder.GetControllerRightHand();

        if (leftController != null && leftController.activeSelf && rightController != null && rightController.activeSelf)
        {
            EventBus.Post(new SceneReadyEvent());
        } else
        {
            Invoke("CheckDevices", 1);
        }
    }

    private void ControllerEnabled(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log(e.controllerIndex + "CONTROLLER STATE ENABLED" + e);
    }

    private void ControllerDisabled(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log(e.controllerIndex + "CONTROLLER STATE DISABLED" + e);
    }
}
