using UnityEngine;
using System.Collections;
using VRTK;

public class SelectionInteractionConcept : MonoBehaviour {

    private ControllerInteractionEventHandler startIdle;

    void Awake()
    {
        EventBus.Register(this);
    }

    public void OnEvent(StartInteractionConceptEvent e)
    {
        if (e.newConcept == InteractionConcept.Selection)
        {
            VRTK_ControllerEvents rightController = VRTK.VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();

            startIdle = new ControllerInteractionEventHandler(StartIdle);
            rightController.TriggerTouchEnd += startIdle;
        }
    }

    public void OnEvent(StopInteractionConceptEvent e)
    {
        if (e.oldConcept == InteractionConcept.Selection)
        {
            VRTK_ControllerEvents rightController = VRTK.VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();

            rightController.TriggerTouchEnd -= startIdle;
        }
    }

    public void StartIdle(object sender, ControllerInteractionEventArgs e)
    {
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Idle));
    }
}
