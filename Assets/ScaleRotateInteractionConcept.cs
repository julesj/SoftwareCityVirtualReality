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

            // set inital scale and show menu
            scaleRotateMenu.SetActive(true);
            float progress = leftController.GetTriggerAxis();
            scaleRotateMenu.gameObject.GetComponent<ScaleAnimator>().SetAnimationProgress(progress);
            /*
            print("start: " + scale);
            scaleRotateMenu.transform.localScale = new Vector3(scale, scale, scale);
            */
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
        float progress = e.buttonPressure;
        scaleRotateMenu.gameObject.GetComponent<ScaleAnimator>().SetAnimationProgress(progress);
        /*
        print("update: " + scale);
        scaleRotateMenu.transform.localScale = new Vector3(scale, scale, scale);
        */
    }

    public void StartIdle(object sender, ControllerInteractionEventArgs e)
    {
        scaleRotateMenu.gameObject.GetComponent<ScaleAnimator>().ResetAnimation();
        scaleRotateMenu.SetActive(false);
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Idle));
    }
}
