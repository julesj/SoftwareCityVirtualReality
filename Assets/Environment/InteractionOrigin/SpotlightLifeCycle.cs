using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpotlightLifeCycle : MonoBehaviour {

    private Light light;
    private Camera[] cams;
    private Transform lightShaft;
	
	void Awake () {
        LifeCycle lifeCycle = FindObjectOfType<LifeCycle>();
        if (lifeCycle != null)
        {
            lifeCycle.OnBeginHandler += OnBegin;
            lifeCycle.OnFinishHandler += OnFinish;
        }
        cams = FindObjectsOfType<Camera>();
        light = GetComponentInChildren<Light>();
        lightShaft = transform.parent.FindChild("LightShaft");
        OnBegin();
    }

    private void OnBegin() {
        GetComponentInChildren<Animator>().SetBool("open", true);
    }


    private void OnFinish()
    {
        GetComponentInChildren<Animator>().SetBool("open", false);
    }

    void Update()
    {
        float val = light.spotAngle / 30;
        float shaftScale = 0.007f * (1 - (val-(1/30f))) + 0.4f * (val - (1 / 30f));
        lightShaft.localScale = new Vector3(shaftScale, 100, shaftScale);
        RenderSettings.ambientLight = new Color(0.01f + 0.2f * val, 0.01f + 0.2f * val, 0.02f + 0.4f * val);
        RenderSettings.fogColor = RenderSettings.ambientLight / 5;
        if (cams != null)
        {
            foreach(Camera cam in cams)
            {
                cam.backgroundColor = RenderSettings.fogColor;
            }
        }
    }

}
