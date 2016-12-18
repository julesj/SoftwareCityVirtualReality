using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpotlightLifeCycle : MonoBehaviour {

    private Light light;
    private Camera[] cams;
    private Transform lightShaft;
	
	void Awake () {
        EventBus.Register(this);
    }

    public void OnEvent(SceneReadyEvent e)
    {
        cams = FindObjectsOfType<Camera>();
        light = GetComponentInChildren<Light>();
        lightShaft = transform.parent.FindChild("LightShaft");
    }

    public void OnEvent(StartPlayingEvent e)
    {
        GetComponentInChildren<Animator>().SetBool("open", true);
    }


    public void OnEvent(StopPlayingEvent e)
    {
        GetComponentInChildren<Animator>().SetBool("open", false);
    }

    void Update()
    {
        if (cams ==  null)
        {
            return;
        }
        float val = (light.spotAngle-1) / 30;
        float shaftScale = 1.1f + 23f*val;
        lightShaft.localScale = new Vector3(shaftScale, 1, shaftScale);
        RenderSettings.ambientLight = new Color(0.03f + 0.2f * val, 0.03f + 0.2f * val, 0.06f + 0.4f * val);
        RenderSettings.fogColor = RenderSettings.ambientLight / 2;
        RenderSettings.fog = false;
        if (cams != null)
        {
            foreach(Camera cam in cams)
            {
                cam.backgroundColor = RenderSettings.fogColor;
            }
        }
    }

}
