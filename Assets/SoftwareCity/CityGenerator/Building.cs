using UnityEngine;
using System.Collections;
using System;

public class Building : MonoBehaviour {

    public BuildingNode node;


    
    private float distStartFade = 2;
    private float distStopFade = 0.75f;

    private int counter;
    private DoNotCollideWithBuildings[] objects;
    private Color originalColor;
    private static Color transparent = new Color(0, 0, 0, 0);
    private bool colorChanged = false;
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
                if (dist < distStartFade && dist > distStopFade)
                {
                    if (!colorChanged)
                    {
                        Material material = GetComponent<MeshRenderer>().material;
                        MakeTransparent(material);
                        //https://forum.unity3d.com/threads/standard-material-shader-ignoring-setfloat-property-_mode.344557/
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
                    MakeOpaque(GetComponent<MeshRenderer>().material);
                }
            }
        }
    }

    private void MakeTransparent(Material material)
    {
        material.SetFloat("_Mode", 2);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }


    private void MakeOpaque(Material material)
    {
        material.SetFloat("_Mode", 1);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 1);
        material.EnableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }
}
