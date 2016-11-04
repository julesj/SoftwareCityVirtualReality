using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AdjustableRoundTable : MonoBehaviour {

    public float height = 1.2f;
    public float radius = 0.45f;

    private AdjustableTableMesh top;
    private AdjustableTableMesh middle;
    private AdjustableTableMesh bottom;

	void Start () {
        top = transform.FindChild("Top").GetComponent<AdjustableTableMesh>();
        middle = transform.FindChild("Middle").GetComponent<AdjustableTableMesh>();
        bottom = transform.FindChild("Bottom").GetComponent<AdjustableTableMesh>();
    }
	
	void Update () {
	    if (NeedsUpdate())
        {
            UpdateMesh();
        }
	}

    private bool NeedsUpdate()
    {
        float actualHeight = top.height + middle.height + bottom.height;
        return (actualHeight != height) || (radius != top.topRadius);
    }

    private void UpdateMesh()
    {
        float tableMiddleHeight = height - top.height - bottom.height;
        float tableTopYPosition = height - top.height;

        top.topRadius = radius;
        top.bottomRadius = radius;
        top.transform.localPosition = new Vector3(0,tableTopYPosition,0);
        middle.height = tableMiddleHeight;

        top.UpdateMesh();
        middle.UpdateMesh();
    }
}
