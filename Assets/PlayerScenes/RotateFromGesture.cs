using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.SceneManagement;

public class RotateFromGesture : MonoBehaviour
{

    private bool palmOpen;
    private bool rotateLeft;
    private bool rotateRight;

    private Controller controller;

    private FloatModel scaleModel;
    private FloatModel rotateModel;

    public float scaleFactor = 0.001f;

    // Use this for initialization
    void Start()
    {
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
    void Update()
    {
        GameObject canvas = GameObject.Find("Canvas_Info");
        if (palmOpen && !canvas)
        {
            Frame frame = controller.Frame();
            Frame beforeFrame = controller.Frame(1);
            List<Hand> hands = frame.Hands;
            Hand useHand = hands[0];
            Hand useBeforeHand = beforeFrame.Hands[0];
            Vector3 palmPosition = useHand.PalmPosition.ToVector3();
            Vector3 palmBeforePosition = useBeforeHand.PalmPosition.ToVector3();
            if (rotateRight && rotateModel)
            {
                float value = rotateModel.GetValue();
                Vector3 diff = palmPosition - palmBeforePosition;
                float distance = diff.x;
                distance = Mathf.Max(0, Mathf.Min(1, distance));
                if (value == 0)
                {
                    rotateModel.SetValue(1f - distance * scaleFactor, true);
                }
                else
                {
                    rotateModel.SetValue(value - distance * scaleFactor, true);
                }
            }
            else if (rotateLeft && rotateModel)
            {
                float value = rotateModel.GetValue();
                Vector3 diff = palmBeforePosition - palmPosition;
                float distance = diff.x;
                distance = Mathf.Max(0, Mathf.Min(1, distance));
                if (value == 1)
                {
                    rotateModel.SetValue(0f + distance*scaleFactor, true);
                }
                else
                {
                    rotateModel.SetValue(value + distance*scaleFactor, true);
                }
            }
        }
    }

    private void RotateRight() { 
}

    public void SetPalmOpen(bool isOpen)
    {
        palmOpen = isOpen;
    }

    public void SetRotateLeft(bool doRotateLeft)
    {
        rotateLeft = doRotateLeft;
    }

    public void SetRotateRight(bool doRotateRight)
    {
        rotateRight = doRotateRight;
    }
}
