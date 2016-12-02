using UnityEngine;
using System.Collections;
using VRTK;

public class SliderControl : MonoBehaviour {

    public delegate void OnValueChangedAction(float value);
    public OnValueChangedAction OnValueChanged;

    public Transform sliderStart;
    public Transform sliderEnd;
    public Transform buttonTransform;

    private bool isGrabbed;
    private Vector3 offset;

    void OnTriggerStay(Collider other)
    {

        ControlTrigger trigger = other.transform.GetComponent<ControlTrigger>();

        if (trigger == null)
        {
            return;
        }

        if (trigger.IsPressed())
        {
            if (!isGrabbed)
            {
                offset = buttonTransform.position - other.transform.position;
                isGrabbed = true;
            }
        } else
        {
            isGrabbed = false;
            return;
        }

        Vector3 contact = other.transform.position;
        Vector3 completeSlide = sliderEnd.position - sliderStart.position;
        Vector3 amountSlide = Vector3.Project((contact + offset) - sliderStart.position, completeSlide.normalized);
        float value = amountSlide.magnitude / completeSlide.magnitude;
        Vector3 buttonPos = sliderStart.position + amountSlide;

        if (value > 1)
        {
            value = 1;
            buttonTransform.position = sliderEnd.position;
        } else if (Vector3.Distance(sliderStart.position, sliderEnd.position) <= Vector3.Distance(sliderEnd.position, buttonPos))
        {
            value = 0;
            buttonTransform.position = sliderStart.position;
        } else
        {
            buttonTransform.position = buttonPos;
        }
        if (OnValueChanged != null)
        {
            OnValueChanged(value);
        }
    }
    
}
