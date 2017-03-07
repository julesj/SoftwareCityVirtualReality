using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleRotateExampleScript : MonoBehaviour {

    private FloatModel scaleModel;
    private FloatModel rotateModel;

    public float scale;
    public float rotation;
    
    void Start () {
        foreach (FloatModel model in FindObjectsOfType<FloatModel>())
        {
            if (model.name.Equals("Scale"))
            {
                scaleModel = model;
            }
            else if (model.name.Equals("Rotate"))
            {
                rotateModel = model;
            }
        }
    }
	
	void Update () {
        scaleModel.SetValue(scale);
        rotateModel.SetValue(rotation);
	}
}
