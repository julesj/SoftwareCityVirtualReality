using UnityEngine;
using System.Collections;

public class SliderControl : MonoBehaviour {

    public delegate void OnValueChangedAction(float value);
    public OnValueChangedAction OnValueChanged;

    public Transform sliderStart;
    public Transform sliderEnd;
    public Transform buttonTransform;

    void OnTriggerStay(Collider other)
    {
        if (!other.transform.GetComponent<ControlTrigger>())
        {
            return;
        }

        Vector3 contact = other.transform.position;
        Vector3 completeSlide = sliderEnd.position - sliderStart.position;
        Vector3 amountSlide = Vector3.Project(contact - sliderStart.position, completeSlide.normalized);
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
