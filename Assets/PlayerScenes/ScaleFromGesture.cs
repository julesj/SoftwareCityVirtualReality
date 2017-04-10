using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.SceneManagement;

public class ScaleFromGesture : MonoBehaviour {

    private bool palmOpen;
    private bool minimize;
    private bool maximize;

    private Controller controller;

    private FloatModel scaleModel;
    private FloatModel rotateModel;

    public float scaleFactor = 0.001f;

	// Use this for initialization
	void Start () {
        controller = new Controller();
        SceneManager.sceneLoaded += SceneLoaded;
	}

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Szene " + scene.name + " wurde geladen");
        if (scene.name.Equals(ClipboardBar.LoadableScenes.ScaleRotateExampleScene.ToString()))
        {
            foreach (FloatModel model in FindObjectsOfType<FloatModel>())
            {
                if (model.name.Equals("Scale"))
                {
                    scaleModel = model;
                }
                if (model.name.Equals("Rotate"))
                {
                    rotateModel = model;
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
		if (palmOpen)
        {
            Frame frame = controller.Frame();
            Frame beforeFrame = controller.Frame(1);
            List<Hand> hands = frame.Hands;
            Hand useHand = hands[0];
            Hand useBeforeHand = beforeFrame.Hands[0];
            Vector3 palmPosition = useHand.PalmPosition.ToVector3();
            Vector3 palmBeforePosition = useBeforeHand.PalmPosition.ToVector3();
            if (minimize && scaleModel)
            {
                float value = scaleModel.GetValue();
                Debug.Log("before value: " + value);
                Vector3 diff = palmPosition - palmBeforePosition;
                Debug.Log("minimieren um: " + diff.y);
                float distance = diff.y;
                distance = Mathf.Max(0, Mathf.Min(1, distance));
                scaleModel.SetValue(value - (distance * scaleFactor));
            } else if (maximize && scaleModel)
            {
                float value = scaleModel.GetValue();
                Vector3 diff = palmBeforePosition - palmPosition;
                Debug.Log("maximieren um: " + diff.y);
                float distance = diff.y;
                distance = Mathf.Max(0, Mathf.Min(1, distance));
                Debug.Log("distance: " + distance + ", neuer Wert: " + value + " + " + distance + " * " + scaleFactor + " = " + (value + distance * scaleFactor));
                scaleModel.SetValue(value + (distance * scaleFactor));
            }
        }
	}

    public void SetPalmOpen(bool isOpen)
    {
        palmOpen = isOpen;
    }

    public void SetMinimize(bool doMinimize)
    {
        minimize = doMinimize;
    }

    public void SetMaximize(bool doMaximize)
    {
        maximize = doMaximize;
    }
}
