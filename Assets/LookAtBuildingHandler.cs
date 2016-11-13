using UnityEngine;
using System.Collections;

public class LookAtBuildingHandler : MonoBehaviour {

    private Ray ray;
    private float timeNextSample;
    public float samplingIntervalInSeconds = 0.25f;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > timeNextSample)
        {
            ray.origin = transform.position;
            ray.direction = transform.forward;
            timeNextSample = Time.time + samplingIntervalInSeconds;

            float nearestDist = float.MaxValue;
            Building selectedBuilding = null;

            foreach (RaycastHit hit in Physics.RaycastAll(ray, 100))
            {
                if (hit.distance > nearestDist)
                {
                    continue;
                }
                nearestDist = hit.distance;
                if (hit.collider.transform.GetComponent<Building>() != null)
                {
                    selectedBuilding = hit.collider.transform.GetComponent<Building>();
                }
            }
            if (selectedBuilding != null)
            {
                transform.FindChild("FileNameLabel").GetComponent<TextMesh>().text = selectedBuilding.node.name;
                transform.FindChild("PathNameLabel").GetComponent<TextMesh>().text = selectedBuilding.node.pathName;
            } else
            {
                transform.FindChild("FileNameLabel").GetComponent<TextMesh>().text = "";
                transform.FindChild("PathNameLabel").GetComponent<TextMesh>().text = "";
            }
        }
	}
}
