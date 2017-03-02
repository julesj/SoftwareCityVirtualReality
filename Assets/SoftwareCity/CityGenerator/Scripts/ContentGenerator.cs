using UnityEngine;
using System.Collections.Generic;

public class ContentGenerator : MonoBehaviour {

    public Transform origin;
	public GameObject buildingPrefab;
	public GameObject streetPrefab;
	public GameObject blockPrefab;
    public string pathSeparator = ".";


    public void GenerateContent(Project project) {
        origin.localScale = Vector3.one;
        foreach (Transform  child in origin) {
			Destroy (child.gameObject);
		}
		Block city = CreateBlock (project.CreateNodeModel (pathSeparator));
		city.gameObject.transform.SetParent (origin, true);
		city.gameObject.transform.position = new Vector3 (0, 0, -city.length / 2);
		float size = Mathf.Max(city.length, city.width);
        origin.localScale = Vector3.one / size;
	}


	private Node CreateModel(int depth) {
		int subTreeCount = (int)(depth / 2f + (Random.value * depth / 2f));
		int leafCount = (int)(Random.value * subTreeCount * 70/depth);
		List<Node> nodes = new List<Node> ();
		for (int i = 0; i < subTreeCount; i++) {
			nodes.Insert((int) (nodes.Count*Random.value), CreateModel (depth-2));
		}

		for (int i = 0; i < leafCount; i++) {
			nodes.Insert((int) (nodes.Count*Random.value), new BuildingNode());
		}

		StreetNode street = new StreetNode (nodes.ToArray());

		return street;
	}

	private Block CreateBlock(Node node) {
		if (node is BuildingNode) {
			return CreateBuildingBlock ((BuildingNode)node);
		} else {
			return CreateStreetBlock ((StreetNode)node);
		}
	}

	private Block CreateBuildingBlock(BuildingNode building) {
		Block result = new Block ();
		result.gameObject = (GameObject) GameObject.Instantiate (buildingPrefab, Vector3.zero, Quaternion.identity);
		result.gameObject.transform.localScale = new Vector3 (building.groundSize, building.height, building.groundSize);
		result.gameObject.transform.Find("Building Object").GetComponent<Renderer> ().material.SetColor ("_Color", new Color(building.color.x, building.color.y, building.color.z));
		result.width = building.groundSize * 1.2f;
		result.length = result.width;
        result.gameObject.GetComponentInChildren<Building>().node = building;
        return result;
	}

	private Block CreateStreetBlock(StreetNode street) {

		GameObject blockGameObject = (GameObject) GameObject.Instantiate (blockPrefab, Vector3.zero, Quaternion.identity);

		int childCount = street.nodes.GetLength (0);

		float leftMaxWidth = 0;
		float rightMaxWidth = 0;
		float offsetLeft = 0;
		float offsetRight = 0;
		Transform leftSide = blockGameObject.transform.Find ("LeftSide").transform;
		Transform rightSide = blockGameObject.transform.Find ("RightSide").transform;
		for (int i = 0; i<childCount; i++) {
			Node node = street.nodes [i];
			Block childBlock = CreateBlock (node);
			GameObject childBlockGameObject = childBlock.gameObject;
			if (i % 2 == 1) {
				childBlockGameObject.transform.RotateAround (Vector3.zero, Vector3.up, -90);
				childBlockGameObject.transform.localPosition = new Vector3 (0, 0, childBlock.width / 2 + offsetLeft);
				offsetLeft += childBlock.width;
				leftMaxWidth = (leftMaxWidth > childBlock.length) ? leftMaxWidth : childBlock.length;
				childBlockGameObject.transform.SetParent (leftSide, false);
			} else {
				childBlockGameObject.transform.RotateAround (Vector3.zero, Vector3.up, 90);
				childBlockGameObject.transform.localPosition = new Vector3 (0, 0, childBlock.width / 2 + offsetRight);
				offsetRight += childBlock.width;
				rightMaxWidth = (rightMaxWidth > childBlock.length) ? rightMaxWidth : childBlock.length;
				childBlockGameObject.transform.SetParent (rightSide, false);
			}
		}

		float midpointOffset = (leftMaxWidth - rightMaxWidth) / 2;

		float length = ((offsetLeft > offsetRight) ? offsetLeft : offsetRight) * 1f;
		float streetWidth = length / 50;
		leftSide.Translate(new Vector3(-streetWidth / 2 + midpointOffset, 0, 0));
		rightSide.Translate(new Vector3(streetWidth / 2 + midpointOffset, 0, 0));

		GameObject streetGameObject = (GameObject) GameObject.Instantiate (streetPrefab, new Vector3(midpointOffset, 0, 0), Quaternion.identity);
		streetGameObject.transform.localScale = new Vector3 (streetWidth, 1, length);
		streetGameObject.transform.SetParent (blockGameObject.transform, true);

		Block result = new Block ();
		result.gameObject = blockGameObject;
		result.width = (leftMaxWidth +streetWidth + rightMaxWidth) * 1f;
		result.length = length;
		return result;
	}

}
