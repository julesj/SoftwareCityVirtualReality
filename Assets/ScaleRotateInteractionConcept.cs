using UnityEngine;
using System.Collections;
using VRTK;

public class ScaleRotateInteractionConcept : MonoBehaviour {

    public GameObject scaleRotateMenu;

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

            // set inital animation progress and show menu
            scaleRotateMenu.SetActive(true);
            scaleRotateMenu.gameObject.GetComponent<ScaleAnimator>().SetAnimationProgress(leftController.GetTriggerAxis());
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
        // update progress of animation
        scaleRotateMenu.gameObject.GetComponent<ScaleAnimator>().SetAnimationProgress(e.buttonPressure);
    }

    public void StartIdle(object sender, ControllerInteractionEventArgs e)
    {
        // reset animation and hide menu
        scaleRotateMenu.gameObject.GetComponent<ScaleAnimator>().ResetAnimation();
        scaleRotateMenu.SetActive(false);

        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Idle));
    }
}
