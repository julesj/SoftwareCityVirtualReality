using UnityEngine;
using System.Collections;
using VRTK;

public class SelectionInteractionConcept : MonoBehaviour {

    private ControllerInteractionEventHandler startIdle;
    private ControllerInteractionEventHandler selectionConfirmed;

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
            selectionConfirmed = new ControllerInteractionEventHandler(SelectionConfirmed);
            rightController.TriggerTouchEnd += startIdle;
            rightController.TouchpadPressed += selectionConfirmed;
        }
    }

    public void OnEvent(StopInteractionConceptEvent e)
    {
        if (e.oldConcept == InteractionConcept.Selection)
        {
            VRTK_ControllerEvents rightController = VRTK.VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();

            rightController.TriggerTouchEnd -= startIdle;
            rightController.TouchpadPressed -= selectionConfirmed;

            Hint.Hide("BuildingSelectionConfirmHint");
        }
    }

    public void StartIdle(object sender, ControllerInteractionEventArgs e)
    {
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Idle));
    }

    public void SelectionConfirmed(object sender, ControllerInteractionEventArgs e)
    {
        EventBus.Post(new Events.BuildingSelectionConfirmedEvent());
    }
}
