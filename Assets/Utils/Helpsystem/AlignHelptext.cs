using UnityEngine;
using System.Collections;
using VRTK;

public class AlignHelptext : MonoBehaviour {


    private Transform camera;
	
    void Start () {
        Show();
	}

    public void Show()
    {
        camera = VRTK_DeviceFinder.HeadsetTransform();
    }
	
	void Update () {
	    if (camera != null)
        {
            transform.rotation = Quaternion.LookRotation((transform.position - camera.position).normalized, Vector3.up);
        }
	}
}
