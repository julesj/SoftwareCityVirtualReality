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
    private List<GameObject> collided;

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
        collided = new List<GameObject> { };
        dontCollide = new List<string> { "Body", "SideA", "SideB", "Canvas_direkt", "[Controller (right)]StraightPointerRenderer_Tracer", "[Controller (left)]StraightPointerRenderer_Tracer" };
        if (!interactableWithLaserpointer)
        {
            dontCollide.Add("[Controller (right)]StraightPointerRenderer_Cursor");
            dontCollide.Add("[Controller (left)]StraightPointerRenderer_Cursor");
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
        if (!dontCollide.Contains(other.gameObject.name) && collided.Count() == 0)
        {
            Debug.Log("OnTriggerEnter: " + other.gameObject.name);
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

            controllerEvents.TriggerClicked -= TriggerListener_AliasUIClickOn;
            controllerEvents.TriggerClicked += TriggerListener_AliasUIClickOn;
            collided.Add(other.gameObject);

            gameObject.GetComponent<Button>().Select();
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(2000);
        }
    }

    private void TriggerListener_AliasUIClickOn(object sender, ControllerInteractionEventArgs e)
    {
        SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(2000);
        if (gameObject)
        {
            gameObject.GetComponent<Button>().onClick.Invoke();
            controllerEvents.TriggerClicked -= TriggerListener_AliasUIClickOn;

            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!dontCollide.Contains(other.gameObject.name) && collided.Contains(other.gameObject))
        {
            Debug.Log("OnTriggerExit: " + other.gameObject.name);
            controllerEvents.TriggerClicked -= TriggerListener_AliasUIClickOn;
            collided.Remove(other.gameObject);

            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}

