using UnityEngine;
using System.Collections;
using VRTK;
using System;

public class SliderControl : MonoBehaviour {

    private enum GrabbedState
    {
        Grabbed,
        Hovered,
        None
    }

    public Transform sliderStart;
    public Transform sliderEnd;
    public Transform buttonTransform;

    public float lightValueOff = 0;
    public float lightValueHovered = 0.1f;
    public float lightValueActive = 1f;

    public float materialEmissionOff = 0;
    public float materialEmissionHovered = 0.1f;
    public float materialEmissionActive = 1f;

    public delegate void OnSliderMove(SliderControl sliderControl);
    public OnSliderMove OnSliderMoveHanders;

    private GrabbedState grabbedState = GrabbedState.None;
    private Vector3 offset;
    private FloatModel model;
    private Light light;
    private Material material;
    private float lastModelValue;

    void Start()
    {
        light = GetComponent<Light>();
        material = GetComponent<MeshRenderer>().material;
        SetHighlightOff();
    }

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

        float delta = Mathf.Abs(lastModelValue - model.GetValue());

        float intensity = delta / 0.05f;

        if (grabbedState == GrabbedState.Grabbed)
        {
            TriggerHapticPulses(0.025f * intensity, 0.075f * intensity);
        }
        lastModelValue = model.GetValue();
    }

    void OnTriggerExit(Collider other)
    {
        ControlTrigger trigger = other.transform.GetComponent<ControlTrigger>();

        if (trigger != null)
        {
            grabbedState = GrabbedState.None;
            SetHighlightOff();
        }
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
            if (grabbedState != GrabbedState.Grabbed)
            {
                offset = buttonTransform.position - other.transform.position;
                grabbedState = GrabbedState.Grabbed;
                SetHighlightGrabbed();
            }
        } else
        {
            if (grabbedState != GrabbedState.Hovered)
            {
                grabbedState = GrabbedState.Hovered;
                SetHighlightHovered();
            }
            return;
        }

        if (OnSliderMoveHanders != null)
        {
            OnSliderMoveHanders(this);
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

    private void SetHighlightOff()
    {
        SetHighlightValues(lightValueOff, materialEmissionOff);
    }

    private void SetHighlightHovered()
    {
        SetHighlightValues(lightValueHovered, materialEmissionHovered);
        TriggerHapticPulses(0.125f, 0.5f);
    }

    private void SetHighlightGrabbed()
    {
        SetHighlightValues(lightValueActive, materialEmissionActive);
        TriggerHapticPulses(0.125f, 1f);
    }

    private void SetHighlightValues(float lightValue, float materialEmissionValue)
    {
        if (light != null)
        {
            light.intensity = lightValue;
        }
        if (material != null)
        {
            material.SetColor("_EmissionColor", material.GetColor("_Color") * materialEmissionValue);
        }
    }

    private void TriggerHapticPulses(float left, float right)
    {
        VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerActions>().TriggerHapticPulse((ushort)(left * 3999));
        VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerActions>().TriggerHapticPulse((ushort)(right * 3999));
    }
}
