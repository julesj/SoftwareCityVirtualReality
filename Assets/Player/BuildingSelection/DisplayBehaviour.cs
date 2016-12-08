using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayBehaviour : MonoBehaviour {
    
	void Start () {
        GetComponent<AnimateThis>().Transformate().FromScale(0).ToScale(1).Duration(1f).Ease(AnimateThis.EasePow2).Start();
	}
	
	public void SetData(Building building, Vector3 startPos, Transform viewport)
    {
        
        Vector3 endPos = viewport.position - Vector3.ProjectOnPlane((viewport.position - startPos).normalized, Vector3.up)*  1.5f;
        Quaternion startRot = Quaternion.LookRotation((endPos - startPos).normalized, Vector3.up);
        Quaternion endRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(viewport.forward, Vector3.up), Vector3.up);

        GetComponent<AnimateThis>().Transformate()
            .FromPosition(startPos)
            .ToPosition(endPos)
            .FromRotation(startRot)
            .ToRotation(endRot)
            .Duration(1).Start();

        BuildingNode node = building.node;
        
        Dictionary<string, string> attributes = node.allAttributes;
        List<string> keys = new List<string>();
        keys.AddRange(attributes.Keys);
        keys.Sort();
        string list = "";
        foreach (string key in keys)
        {
            list += key + ": " + attributes[key] + "\n";
        }

        transform.FindChild("PackageLabel").GetComponent<TextMesh>().text = node.pathName;
        transform.FindChild("NameLabel").GetComponent<TextMesh>().text = node.name;
        transform.FindChild("MetricsLabel").GetComponent<TextMesh>().text = list;

    }
}
