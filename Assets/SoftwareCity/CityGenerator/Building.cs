using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

    public BuildingNode node;


    
    private float distStartFade = 2;
    private float distStopFade = 0.75f;

    private int counter = (int) (Random.value * int.MaxValue);
    private DoNotCollideWithBuildings[] objects;
    private Color originalColor;
    private static Color transparent = new Color(0, 0, 0, 0);
    private bool colorChanged = false;
    public void Awake()
    {
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
                if (dist < distStartFade && dist > distStopFade)
                {
                    if (!colorChanged)
                    {
                        originalColor = GetComponent<MeshRenderer>().material.color;
                        colorChanged = true;
                    }
                    Color col = originalColor;
                    col.a = (dist - distStopFade) / (distStartFade - distStopFade);
                    GetComponent<MeshRenderer>().material.color = col;
                } else if (dist < distStopFade)
                {
                    if (!colorChanged)
                    {
                        originalColor = GetComponent<MeshRenderer>().material.color;
                        colorChanged = true;
                    }
                    GetComponent<MeshRenderer>().material.color = transparent;
                } else if (colorChanged)
                {
                    colorChanged = false;
                    GetComponent<MeshRenderer>().material.color = originalColor;
                }
            }
        }
    }

}
