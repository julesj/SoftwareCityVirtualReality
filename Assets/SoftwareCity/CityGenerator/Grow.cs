using UnityEngine;


public class Grow : MonoBehaviour {

    private SliderControl scaleSliderControl;
    private  SliderControl rotationSliderControl;
    public float minScale = 1;
    public float maxScale = 100;
    private float scale;
    //private float scaleToReach;
    private float animateTimeStart;
    private float animateTimeEnd;
    private bool animating;

    void Awake()
    {
        EventBus.Register(this);
    }

    public void OnEvent(StartPlayingEvent e)
    {
        scale = minScale;
        //scaleToReach = minScale;
        /*animating = true;
        animateTimeStart = Time.time;
        animateTimeEnd = animateTimeStart + 3;*/
    }

    public void OnEvent(StopPlayingEvent e)
    {
        scale = 0;
        //scaleToReach = 0;
        /*animating = true;
        animateTimeStart = Time.time;
        animateTimeEnd = animateTimeStart + 2;*/
    }

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
        transform.localScale = Vector3.zero;
       
    }

    public void SetScaleValue(float value)
    {
        /*scale = CalcScale(); */
		scale = (minScale + (maxScale - minScale) * value* value* value* value* value* value* value);
        transform.localScale = Vector3.one * scale;
        //animating = true;
        //animateTimeStart = Time.time;
        //animateTimeEnd = animateTimeStart + 1;*/

    }

    private void SetRotationValue(float value)
    {
        transform.localRotation = Quaternion.Euler(0, value * 360, 0);
    }

   /* private float CalcScale()
    {
        if (!animating)
        {
            return scale;
        }
        float t = (Time.time - animateTimeStart) / (animateTimeEnd - animateTimeStart);
        if (t > 1)
        {
            t = 1;
        }
        t = (Mathf.Cos(Mathf.PI * t) - 1) / -2;
        return scale * (1 - t) + scaleToReach * t;
    }*/

    void Update()
    {
       /* if (animating)
        {
            if (Time.time > animateTimeEnd)
            {
                animating = false;
                scale = scaleToReach;
                transform.localScale = Vector3.one * scale;
            } else
            {
                transform.localScale = Vector3.one * CalcScale();
            }

        }*/
    }
}
