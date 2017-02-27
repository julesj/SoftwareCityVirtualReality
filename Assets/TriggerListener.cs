using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using VRTK;

public class TriggerListener : MonoBehaviour
{
    //Dieses Skript gehört auf Buttons!
    //Zusätzlich muss der Button einen Rigidbody mit IsKinematic und ohne Gravity und BoxCollider (ohne Trigger) und Controller Interact Touch besitzen

    //Ausprobieren ob dieses Skript dann auch mit den Händen benutzbar ist -> Collider auf Händen gibt es doch bestimmt schon...

    VRTK_ControllerEvents controllerEvents;
    SteamVR_TrackedObject trackedObj;
    List<string> dontCollide;

    public Boolean interactableWithLaserpointer;
    public Boolean interactableWithController;
    public Boolean interactableWithHands;

    private void Awake()
    {
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;

        BoxCollider boxcollider = gameObject.AddComponent<BoxCollider>();
        boxcollider.isTrigger = false;
        boxcollider.size = new Vector3(gameObject.GetComponent<RectTransform>().rect.width, gameObject.GetComponent<RectTransform>().rect.height, 1);
    }

    private void Start()
    {
        dontCollide = new List<string> { "Body", "SideA", "SideB", "Canvas_direkt", "[Controller (right)]BasePointer_SimplePointer_PointerTip", "[Controller (left)]BasePointer_SimplePointer_PointerTip" };
        if (!interactableWithLaserpointer)
        {
            dontCollide.Add("[Controller (right)]Basepointer_SimplePointer_Pointer");
            dontCollide.Add("[Controller (left)]Basepointer_SimplePointer_Pointer");
        }
        if (!interactableWithController)
        {
            dontCollide.Add("Head");
        }
        if (!interactableWithHands)
        {
            //TODO
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!dontCollide.Contains(other.gameObject.name))
        {
            controllerEvents = other.gameObject.GetComponentInParent<VRTK_ControllerEvents>();
            trackedObj = other.gameObject.GetComponentInParent<SteamVR_TrackedObject>();
            if (controllerEvents == null && interactableWithLaserpointer) //Bei Laserpointer
            {
                string name = other.gameObject.name;
                string pattern = @"\[(.+)]";

                Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                Match m = r.Match(name);
                Debug.Log(m.Groups[1].Value);
                controllerEvents = GameObject.Find(m.Groups[1].Value).GetComponent<VRTK_ControllerEvents>();
                trackedObj = GameObject.Find(m.Groups[1].Value).GetComponent<SteamVR_TrackedObject>();
            }

            controllerEvents.AliasUIClickOn += TriggerListener_AliasUIClickOn;

            gameObject.GetComponent<Button>().Select();
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(2000);
        }
    }

    private void TriggerListener_AliasUIClickOn(object sender, ControllerInteractionEventArgs e)
    {
        SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(2000);
        gameObject.GetComponent<Button>().onClick.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        //Laserpointer hört auf abfangen?
    }

    private void OnTriggerExit(Collider other)
    {
        if(!dontCollide.Contains(other.gameObject.name))
        {
            controllerEvents.AliasUIClickOn -= TriggerListener_AliasUIClickOn;

            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}

