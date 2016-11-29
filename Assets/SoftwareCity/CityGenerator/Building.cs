using UnityEngine;
using System.Collections;
using System;

public class Building : MonoBehaviour {

    public BuildingNode node;

    private float distStartFade = 2;

    private int counter;
    private DoNotCollideWithBuildings[] objects;
    public Material transparentMaterial;
    private Material originalMaterial;
    private bool isTransparent;

    public void Awake()
    {
        counter = (int)(UnityEngine.Random.value * int.MaxValue);
        EventBus.Register(this);
    }

    public void OnEvent(SceneReadyEvent e)
    {
        objects = FindObjectsOfType<DoNotCollideWithBuildings>();
    }

    public void Update()
    {
        counter = (counter + 1) % 5;
        if (counter == 0 && objects != null)
        {
            foreach(DoNotCollideWithBuildings obj in objects)
            {
                Vector3 p1 = transform.position;
                Vector3 p2 = obj.transform.position;
                p1.y = 0;
                p2.y = 0;
                float dist = Vector3.Distance(p1, p2) - transform.lossyScale.x/2;
                if (dist < distStartFade) {
                    if (!isTransparent)
                    {
                        originalMaterial = gameObject.GetComponent<Renderer>().material;
                        gameObject.GetComponent<Renderer>().material = transparentMaterial;
                        transparentMaterial.color = originalMaterial.color;
                        isTransparent = true;
                    }
                } else {
                    if (isTransparent)
                    {
                        gameObject.GetComponent<Renderer>().material = originalMaterial;
                        isTransparent = false;
                    }
                }
            }
        }
    }
}
