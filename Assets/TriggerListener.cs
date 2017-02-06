using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text.RegularExpressions;

namespace VRTK
{
    public class TriggerListener : MonoBehaviour
    {
        //Dieses Skript gehört auf Buttons!
        //Zusätzlich muss der Button einen Rigidbody mit IsKinematic und ohne Gravity und BoxCollider (ohne Trigger) und Controller Interact Touch besitzen

        //Ausprobieren ob dieses Skript dann auch mit den Händen benutzbar ist -> Collider auf Händen gibt es doch bestimmt schon...

        GameObject container; //Container der Collider der Controller
        GameObject laserpointerR;
        VRTK_ControllerEvents controllerEvents;
        SteamVR_TrackedObject trackedObj;
        bool isTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name != "Canvas_direkt")
            {
                container = GameObject.Find("VRTK_ControllerCollidersContainer"); //Evtl. nur Head benutzen oder container ist schon compound collider
                laserpointerR = GameObject.Find("[Controller (right)]BasePointer_SimplePointer_PointerTip");
                controllerEvents = other.gameObject.GetComponentInParent<VRTK_ControllerEvents>();
                trackedObj = other.gameObject.GetComponentInParent<SteamVR_TrackedObject>();
                if (controllerEvents == null) //Bei Laserpointer
                {
                    string name = other.gameObject.name;
                    string pattern = @"\[(.+)]";

                    Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                    Match m = r.Match(name);
                    Debug.Log(m.Groups[1].Value);
                    controllerEvents = GameObject.Find(m.Groups[1].Value).GetComponent<VRTK_ControllerEvents>();
                    trackedObj = GameObject.Find(m.Groups[1].Value).GetComponent<SteamVR_TrackedObject>();
                }

                controllerEvents.AliasUIClickOn += TriggerListener_AliasUIClickOn; //Wird noch zu oft ausgeführt, da vermutlich zu oft hinzugfügt wird

                gameObject.GetComponent<Button>().Select();
                SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(2000);
            }
        }

        private void TriggerListener_AliasUIClickOn(object sender, ControllerInteractionEventArgs e)
        {
            Debug.Log("Pressed");
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(2000);
            gameObject.GetComponent<Button>().onClick.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            //Laserpointer hört auf abfangen?
        }

        private void OnTriggerExit(Collider other)
        {
            controllerEvents.AliasUIClickOn -= TriggerListener_AliasUIClickOn;

            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
