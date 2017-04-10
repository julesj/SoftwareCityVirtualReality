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
        if (palmOpen)
        {
            Frame frame = controller.Frame();
            Frame beforeFrame = controller.Frame(1);
            List<Hand> hands = frame.Hands;
            Hand useHand = hands[0];
            Hand useBeforeHand = beforeFrame.Hands[0];
            Vector3 palmPosition = useHand.PalmPosition.ToVector3();
            Vector3 palmBeforePosition = useBeforeHand.PalmPosition.ToVector3();
            if (rotateLeft && rotateModel)
            {
                float value = rotateModel.GetValue();
                Debug.Log("before value: " + value);
                Vector3 diff = palmPosition - palmBeforePosition;
                Debug.Log("minimieren um: " + diff.x);
                float distance = diff.x;
                distance = Mathf.Max(0, Mathf.Min(1, distance));
                if (value == 0)
                {
                    rotateModel.SetValue(1f - distance * scaleFactor, false);
                }
                else
                {
                    rotateModel.SetValue(value - distance * scaleFactor, false);
                }
            }
            else if (rotateRight && rotateModel)
            {
                float value = rotateModel.GetValue();
                Vector3 diff = palmBeforePosition - palmPosition;
                Debug.Log("maximieren um: " + diff.x);
                float distance = diff.x;
                distance = Mathf.Max(0, Mathf.Min(1, distance));
                if (value == 1)
                {
                    rotateModel.SetValue(0f + distance*scaleFactor, false);
                }
                else
                {
                    rotateModel.SetValue(value + distance*scaleFactor, false);
                }
                Debug.Log("distance: " + distance + ", neuer Wert: " + value + " + " + distance + " * " + scaleFactor + " = " + (value + distance * scaleFactor));
            }
        }
    }

    public void SetPalmOpen(bool isOpen)
    {
        palmOpen = isOpen;
        Debug.Log("palm: " + palmOpen);
    }

    public void SetRotateLeft(bool doRotateLeft)
    {
        rotateLeft = doRotateLeft;
        Debug.Log(rotateLeft);
    }

    public void SetRotateRight(bool doRotateRight)
    {
        rotateRight = doRotateRight;
    }
}
