using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {
    public string name;
}

public class StreetNode : Node {
	public Node[] nodes;
   
	public StreetNode(params Node[] nodes) {
		this.nodes = nodes;
	}
}

public class BuildingNode : Node {
    public string pathName;
    public string groundMappedAttribute;
    public string heightMappedAttribute;
    public string colorMappedAttribute;
    public Dictionary<string, string> allAttributes = new Dictionary<string, string>();

    public float groundSize = Mathf.Pow(Random.value, 2)*10;
	public float height = 1f + Mathf.Pow(Random.value, 2) * 50;
	public Vector3 color = new Vector3 (0.1f+Random.value*0.3f, 0.1f+Random.value*0.3f, 0.1f+Random.value*0.3f);
}

public class Block {
	public GameObject gameObject;
	public float width;
	public float length;

}