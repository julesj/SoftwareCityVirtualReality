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
            float newRotation = transform.localRotation.y;
            rotateModel.SetValue(newRotation / 360, sendEvent: false);
            Debug.Log("newRotation: " + newRotation + "localRotation: " + transform.localRotation);
        } else
        {
            transform.localRotation = Quaternion.Euler(0, value * 360, 0);
        }
    }

    public void SetScaleValue(float value, bool scaleAboutUser = false)
    {
        float scale = (minScale + (maxScale - minScale) * Mathf.Pow(value, 5));
        float beforeScale = transform.localScale.x;
        transform.localScale = Vector3.one * scale;
        if (scaleAboutUser)
        {
            float afterScale = transform.localScale.x;
            float scaleDiff = afterScale / beforeScale;
            Vector3 diff = transform.position - headsetTransform.position;
            Vector3 endPos = (diff * scaleDiff) + headsetTransform.position;
            transform.position = new Vector3(endPos.x, transform.position.y, endPos.z);
        }
    }

}
