using UnityEngine;


public class Grow : MonoBehaviour {

    public float minScale = 1;
    public float maxScale = 100;
    
    private FloatModel scaleModel;
    private FloatModel rotateModel;

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

    private void OnScaleChanged(FloatModel scale)
    {
        SetScaleValue(scale.GetValue());
    }

    private void OnRotateChanged(FloatModel rotate)
    {
        SetRotationValue(rotate.GetValue());
    }

    public void OnEvent(Events.ResetPlayerEvent e)
    {
        scaleModel.SetValue(0.1f);
        rotateModel.SetValue(0);
    }

    private void SetRotationValue(float value)
    {
        transform.localRotation = Quaternion.Euler(0, value * 360, 0);
    }

    private void SetScaleValue(float value)
    {
        float scale = (minScale + (maxScale - minScale) * Mathf.Pow(value, 5));
        transform.localScale = Vector3.one * scale;
    }

}
