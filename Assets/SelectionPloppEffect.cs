using UnityEngine;
using System.Collections;

public class SelectionPloppEffect : MonoBehaviour {

	void Start () {
        GetComponent<AnimateThis>().Transformate().Duration(0.12f).FromScale(new Vector3(1.1f, 1.1f, 1.1f)).ToScale(new Vector3(1.01f, 1.01f, 1.01f)).Ease(AnimateThis.EaseSmooth).Start();
	}
	
}
