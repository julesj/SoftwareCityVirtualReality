using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivate : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Wait());
	}
	
	IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
