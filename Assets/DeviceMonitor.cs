using UnityEngine;
using System.Collections;
using VRTK;

public class DeviceMonitor : MonoBehaviour {
    
	void Awake () {
        CheckDevices();
	}
	
	private void CheckDevices()
    {
        GameObject leftController = VRTK_DeviceFinder.GetControllerLeftHand();
        GameObject rightController = VRTK_DeviceFinder.GetControllerRightHand();

        if (leftController.activeSelf && rightController.activeSelf)
        {
            EventBus.Post(new SceneReadyEvent());
        } else
        {
            Invoke("CheckDevices", 1);
        }
    }
}
