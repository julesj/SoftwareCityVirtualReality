﻿using UnityEngine;
using System.Collections;

public class KeyListener : MonoBehaviour {
	
	void Update () {
	    if (Input.GetKeyDown(KeyCode.R))
        {
            GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.Restart);
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.FinishCalibrating);
            GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.StartCalibrating);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.StartPlaying);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.StopPlaying);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            FindObjectOfType<FloatModel>().SetValue(0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            EventBus.Post(new Events.BuildingSelectionConfirmedEvent());
        }
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            FindObjectOfType<FloatModel>().SetValue(0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Selection));
        }
    }
}
