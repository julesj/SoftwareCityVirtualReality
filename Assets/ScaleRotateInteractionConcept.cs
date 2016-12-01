using UnityEngine;
using System.Collections;
using VRTK;

public class ScaleRotateInteractionConcept : MonoBehaviour {

    public GameObject rotationMenu;

    private ControllerInteractionEventHandler showScaleRotateMenu;
    private ControllerInteractionEventHandler startIdle;

    void Awake () {
        EventBus.Register(this);
	}

    public void OnEvent(StartInteractionConceptEvent e)
    {
        if (e.newConcept == InteractionConcept.ScaleRotate)
        {
            VRTK_ControllerEvents leftController = VRTK.VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerEvents>();

            showScaleRotateMenu = new ControllerInteractionEventHandler(UpdateScaleRotateMenu);
            leftController.TriggerAxisChanged += showScaleRotateMenu;


            startIdle = new ControllerInteractionEventHandler(StartIdle);
            leftController.TriggerTouchEnd += startIdle;

            // set inital scale and show menu
            float scale = leftController.GetTriggerAxis();
            print("start: " + scale);
            rotationMenu.transform.localScale = new Vector3(scale, scale, scale);
            rotationMenu.SetActive(true);
        }
    }

    public void OnEvent(StopInteractionConceptEvent e)
    {
        if (e.oldConcept == InteractionConcept.ScaleRotate)
        {
            VRTK_ControllerEvents leftController = VRTK.VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerEvents>();
            
            leftController.TriggerAxisChanged -= showScaleRotateMenu;
            leftController.TriggerTouchEnd -= startIdle;
        }
    }

    public void UpdateScaleRotateMenu(object sender, ControllerInteractionEventArgs e)
    {
        float scale = e.buttonPressure;
        print("update: " + scale);
        rotationMenu.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void StartIdle(object sender, ControllerInteractionEventArgs e)
    {
        rotationMenu.SetActive(false);
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Idle));
    }
}
