using UnityEngine;
using System.Collections;

public class TriggerBinder : MonoBehaviour {

	public Transform attachPoint;
	public Transform triggerObject;

	void Update () {
		triggerObject.position = attachPoint.position;
	}
}
