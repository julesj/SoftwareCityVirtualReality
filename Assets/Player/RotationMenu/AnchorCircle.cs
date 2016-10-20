using UnityEngine;
using System.Collections;

public class AnchorCircle : MonoBehaviour {

	public Vector3 rotationAxis = Vector3.up;
	public float radius = 0.1f;
	public int numberOfAnchorPoints = 4;
	public float rotationAngle = 0;

	private GameObject[] anchorPoints; 

	private float angle = 90.0f;
	private bool rotating = false;

	void Awake () {
		angle = 360.0f / numberOfAnchorPoints;
		anchorPoints = new GameObject[numberOfAnchorPoints];

		Vector3 center = transform.position;

		for (int i = 0; i < numberOfAnchorPoints; i++) {
			Vector3 position = PositionOnCircle (center, radius, angle * i);
			GameObject gameObject = new GameObject ();
			gameObject.transform.parent = transform;
			gameObject.transform.position = position;
			gameObject.transform.rotation = Quaternion.LookRotation(position - center);
			anchorPoints[i] = gameObject;
		}

	}

	void Update () {
		if (rotating)
		{
			Vector3 to = rotationAxis * angle;
			if (Vector3.Distance(transform.eulerAngles, to) > 0.01f)
			{
				transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, Time.deltaTime);
			}
			else
			{
				transform.eulerAngles = to;
				rotating = false;
			}
		}
	}

	private Vector3 PositionOnCircle(Vector3 center, float radius, float angle) {
		Vector3 position;
		position.x = center.x + radius * Mathf.Sin (angle * Mathf.Deg2Rad);
		position.y = center.y;
		position.z = center.z + radius * Mathf.Cos (angle * Mathf.Deg2Rad);
		return position;
	}

	public void SetGameObjectAtAnchorpoint(uint number, GameObject gameObject) {
		if (number > anchorPoints.Length) {
			return;
		}
		gameObject.transform.position = anchorPoints [number].transform.position;
		gameObject.transform.rotation = anchorPoints [number].transform.rotation;
		gameObject.transform.parent = anchorPoints [number].transform;
	}

	public void RotateToAnchorPoint(int number) {
		angle = (360.0f / numberOfAnchorPoints) * number;
		RotateToAngle (angle);
	}

	public void RotateToAngle(float angle) {
		this.angle = angle;
		rotating = true;
	}
}
