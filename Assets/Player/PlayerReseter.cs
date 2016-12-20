using UnityEngine;
using System.Collections;
using VRTK;

public class PlayerReseter : MonoBehaviour {
    
	void Awake() {
        EventBus.Register(this);
	}
	
	public void OnEvent(Events.ResetPlayerEvent e)
    {
        VRTK_DeviceFinder.PlayAreaTransform().position = Vector3.zero;
    }
}
