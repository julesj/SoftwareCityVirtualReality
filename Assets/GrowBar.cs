using UnityEngine;
using VRTK;


public class GrowBar : MonoBehaviour
{

    public float minScale = 1;
    public float maxScale = 100;

    private FloatModel scaleModel;
    private FloatModel rotateModel;

    private Transform headsetTransform;

    void Awake()
    {
        EventBus.Register(this);
        foreach (FloatModel model in FindObjectsOfType<FloatModel>())
        {
            if (model.name.Equals("Scale"))
            {
                scaleModel = model;
                scaleModel.onFloatModelValueChangedHandler += OnScaleChanged;
            }
            else if (model.name.Equals("Rotate"))
            {
                rotateModel = model;
                rotateModel.onFloatModelValueChangedHandler += OnRotateChanged;
            }
        }
    }

    void Start()
    {
        SetRotationValue(rotateModel.GetValue());
        SetScaleValue(scaleModel.GetValue());
    }

    private void Update()
    {
        if (!headsetTransform || headsetTransform.position != Vector3.zero)
        {
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
        }
    }

    private void OnScaleChanged(FloatModel scale, bool scaleAboutUser = false)
    {
        SetScaleValue(scale.GetValue(), scaleAboutUser);
    }

    private void OnRotateChanged(FloatModel rotate, bool rotateAboutUser = false)
    {
        SetRotationValue(rotate.GetValue(), rotateAboutUser);
    }

    public void OnEvent(Events.ResetPlayerEvent e)
    {
        scaleModel.SetValue(0.1f);
        rotateModel.SetValue(0);
    }

    public void SetRotationValue(float value, bool rotateAboutUser = false)
    {
        if (rotateAboutUser)
        {
            transform.RotateAround(headsetTransform.position, Vector3.up, value*360);
        } else
        {
            transform.localRotation = Quaternion.Euler(0, value * 360, 0);
        }
    }

    public void SetScaleValue(float value, bool scaleAboutUser = false)
    {
        if (scaleAboutUser)
        {
            transform.parent = headsetTransform;
        }
        float scale = (minScale + (maxScale - minScale) * Mathf.Pow(value, 5));
        transform.localScale = Vector3.one * scale;
        if (scaleAboutUser)
        {
            transform.parent = null;
        }
    }

}
