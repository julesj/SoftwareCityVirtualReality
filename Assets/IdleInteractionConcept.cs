using UnityEngine;
using System.Collections;
using VRTK;

public class IdleInteractionConcept : MonoBehaviour {

    private ControllerInteractionEventHandler startScaleRotate;
    private ControllerInteractionEventHandler startSelectNavigate;

    void Awake () {
        EventBus.Register(this);
	}

    public void OnEvent(StartInteractionConceptEvent e)
    {
        if (e.newConcept == InteractionConcept.Idle)
        {
            VRTK_ControllerEvents leftController = VRTK.VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerEvents>();
            VRTK_ControllerEvents rightController = VRTK.VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();

            startScaleRotate = new ControllerInteractionEventHandler(StartScaleRotate);
            leftController.TriggerTouchStart += startScaleRotate;

            
            startSelectNavigate = new ControllerInteractionEventHandler(StartSelectNavigate);
            rightController.TriggerTouchStart += startSelectNavigate;
        }
    }

    public void OnEvent(StopInteractionConceptEvent e)
    {
        if (e.oldConcept == InteractionConcept.Idle)
        {
            VRTK_ControllerEvents leftController = VRTK.VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerEvents>();
            VRTK_ControllerEvents rightController = VRTK.VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();

            leftController.TriggerTouchStart -= startScaleRotate;
            rightController.TriggerTouchStart -= startSelectNavigate;
        }
    }

    public void StartScaleRotate(object sender, ControllerInteractionEventArgs e)
    {
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.ScaleTranslate));
    }

    public void StartSelectNavigate(object sender, ControllerInteractionEventArgs e)
    {
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Selection));
    }

}
