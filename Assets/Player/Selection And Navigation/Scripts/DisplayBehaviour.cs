using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayBehaviour : MonoBehaviour {
    
	void Start () {
        EventBus.Register(this);
        GetComponent<AnimateThis>().Transformate().FromScale(0).ToScale(1).Duration(1f).Ease(AnimateThis.EasePow2).Start();
	}
	
	public void SetData(Building building, Vector3 startPos, Transform viewport)
    {
        Vector3 endPos = viewport.position - Vector3.ProjectOnPlane((viewport.position - startPos).normalized, Vector3.up)* 2f;
        Quaternion startRot = Quaternion.LookRotation((endPos - startPos).normalized, Vector3.up);
        Quaternion endRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(viewport.forward, Vector3.up), Vector3.up);

        transform.position = startPos;
        transform.localScale = Vector3.zero;

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

        transform.Find("PackageLabel").GetComponent<TextMesh>().text = node.pathName;
        transform.Find("NameLabel").GetComponent<TextMesh>().text = node.name;
        transform.Find("MetricsLabel").GetComponent<TextMesh>().text = list;
    }

    public void OnEvent(Events.ClearDisplayEvent e)
    {
        GetComponent<AnimateThis>().Transformate()
            .ToScale(new Vector3(1.2f, 0, 1.2f))
            .Duration(0.25f)
            .OnEnd(DestroyMe)
            .Start();
    }

    private void DestroyMe()
    {
        EventBus.Unregister(this);
        Destroy(gameObject);
    }
}
