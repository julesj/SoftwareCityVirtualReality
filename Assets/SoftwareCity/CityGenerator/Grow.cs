using UnityEngine;


public class Grow : MonoBehaviour {

    private SliderControl scaleSliderControl;
    private  SliderControl rotationSliderControl;
    public float minScale = 1;
    public float maxScale = 100;
    
    public void AddScaleSliderControl(SliderControl scaleSliderControl)
    {
        this.scaleSliderControl = scaleSliderControl;
        scaleSliderControl.OnValueChanged += SetScaleValue;
    }

    public void AddRotationSliderControl(SliderControl rotationSliderControl)
    {
        this.rotationSliderControl = rotationSliderControl;
        rotationSliderControl.OnValueChanged += SetRotationValue;
    }

    void Start()
    {
        transform.localScale = Vector3.one * minScale;
       
    }

    private void SetScaleValue(float value)
    {
		transform.localScale = Vector3.one * (minScale + (maxScale - minScale) * value* value* value* value* value* value* value);
    }

    private void SetRotationValue(float value)
    {
        transform.localRotation = Quaternion.Euler(0, value * 360, 0);
    }
}
