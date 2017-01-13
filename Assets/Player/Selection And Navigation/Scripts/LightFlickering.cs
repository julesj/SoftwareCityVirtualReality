using UnityEngine;
using System.Collections;

public class LightFlickering : MonoBehaviour {

    private Light light;

    public float baseLight = 1;
    public float variance = 0.5f;

    void Start () {
        light = GetComponent<Light>();
	}
	
	void Update () {
        light.intensity = baseLight + (Random.value * 2 - 1) * variance;
	}
}
