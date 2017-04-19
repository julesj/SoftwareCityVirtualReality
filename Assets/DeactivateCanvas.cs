using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateCanvas : MonoBehaviour {

    public void deactivateCanvas()
    {
        GameObject.Find("Canvas_Info").gameObject.SetActive(false);
        Debug.Log("Canvas is deactivated");
    }
}
