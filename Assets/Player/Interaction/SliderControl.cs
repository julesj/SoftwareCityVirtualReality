using UnityEngine;
using System.Collections;
using VRTK;
using System;

public class SliderControl : MonoBehaviour {
    public Transform sliderStart;
    public Transform sliderEnd;
    public Transform buttonTransform;

    private bool isGrabbed;
    private Vector3 offset;
    private FloatModel model;

    public void SetModel(FloatModel model)
    {
        if (this.model != null)
        {
            this.model.onFloatModelValueChangedHandler -= OnModelChanged;
        }
        this.model = model;
        this.model.onFloatModelValueChangedHandler += OnModelChanged;
        SetUi();
    }

    private void OnModelChanged(FloatModel model)
    {
        SetUi();
    }

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
        if (Vector3.Distance(sliderStart.position + amountSlide, sliderEnd.position) > Vector3.Distance(sliderStart.position, sliderEnd.position))
        {
            value = 0;
        } else if (value > 1)
        {
            value = 1;
        }
        model.SetValue(value);
    }
    
    private void SetUi()
    {
        float value = model.GetValue();
        Vector3 completeSlide = sliderEnd.localPosition - sliderStart.localPosition;
        Vector3 buttonPos = sliderStart.localPosition + completeSlide * value;

        if (value > 1)
        {
            value = 1;
            buttonTransform.localPosition = sliderEnd.localPosition;
        }
        else if (Vector3.Distance(sliderStart.localPosition, sliderEnd.localPosition) <= Vector3.Distance(sliderEnd.localPosition, buttonPos))
        {
            value = 0;
            buttonTransform.localPosition = sliderStart.localPosition;
        }
        else
        {
            buttonTransform.localPosition = buttonPos;
        }
    }
}
