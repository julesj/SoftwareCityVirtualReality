using UnityEngine;
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
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            FindObjectOfType<Grow>().SetScaleValue(1f);
        }
    }
}
