using UnityEngine;


public class Grow : MonoBehaviour {

    private SliderControl scaleSliderControl;
    private  SliderControl rotationSliderControl;
    public float minScale = 1;
    public float maxScale = 100;
    
    private float animateTimeStart;
    private float animateTimeEnd;
    private bool animating;
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

    private void OnScaleChanged(FloatModel model)
    {
        float value = model.GetValue();
        float scale = (minScale + (maxScale - minScale) * Mathf.Pow(value, 5));
        transform.localScale = Vector3.one * scale;
    }

    private void OnRotateChanged(FloatModel rotate)
    {
        float value = rotate.GetValue();
        transform.localRotation = Quaternion.Euler(0, value * 360, 0);
    }

    public void OnEvent(StartPlayingEvent e)
    {
        scaleModel.SetValue(0.1f);
        rotateModel.SetValue(0);
    }

    public void OnEvent(StopPlayingEvent e)
    {
        scaleModel.SetValue(0);
        rotateModel.SetValue(0);
    }

    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    private void SetRotationValue(float value)
    {
        transform.localRotation = Quaternion.Euler(0, value * 360, 0);
    }

}
