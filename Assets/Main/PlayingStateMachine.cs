using UnityEngine;
using System.Collections;
using System;

public class PlayingStateMachine : MonoBehaviour {

    private State currentState;

	// Use this for initialization
	void Start () {
        currentState = new InitState();
        currentState.OnEnterState();
	}

    public void PostStateEvent(StateEvent stateEvent)
    {
        State nextState = currentState.GetNextState(stateEvent);
        if (!nextState.GetType().Equals(currentState.GetType()))
        {
            currentState.OnExitState();
            currentState = nextState;
            currentState.OnEnterState();
            Debug.Log("Current playing state is " + currentState.GetType());
        }
    }
}

public enum StateEvent
{
    Restart,
    StartCalibrating,
    FinishCalibrating,
    StartPlaying,
    StopPlaying
}

public interface State
{
    State GetNextState(StateEvent stateEvent);
    void OnEnterState();
    void OnExitState();
}

public class InitState : State
{
    public State GetNextState(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Restart)
        {
            return new RestartState();
        }
        else if (stateEvent == StateEvent.StartCalibrating)
        {
            return new CalibratingState();
        }
        return this;
    }

    public void OnEnterState()
    {
    }

    public void OnExitState()
    {
    }
}

public class CalibratingState : State
{
    public State GetNextState(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Restart)
        {
            return new RestartState();
        } else if (stateEvent == StateEvent.FinishCalibrating)
        {
            return new WaitingForPlayingState();
        }
        return this;
    }

    public void OnEnterState()
    {
        // Activate Interaction Calibration
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Calibration));
    }

    public void OnExitState()
    {
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Nothing));
    }
}

public class WaitingForPlayingState : State
{
    public State GetNextState(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Restart)
        {
            return new RestartState();
        }
        else if (stateEvent == StateEvent.StartPlaying)
        {
            return new PlayingState();
        }
        return this;
    }

    public void OnEnterState()
    {
    }

    public void OnExitState()
    {
    }
}

public class PlayingState : State
{
    public State GetNextState(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Restart)
        {
            return new RestartState();
        }
        else if (stateEvent == StateEvent.StopPlaying)
        {
            return new WaitingForPlayingState();
        }
        return this;
    }

    public void OnEnterState()
    {
        EventBus.Post(new StartPlayingEvent());
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.ScaleTranslate));
    }

    public void OnExitState()
    {
        EventBus.Post(new StopPlayingEvent());
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Nothing));
    }
}

public class RestartState : State
{
    public State GetNextState(StateEvent stateEvent)
    {
        return this;
    }

    public void OnEnterState()
    {
        GameObject.FindObjectOfType<LifeCycle>().Restart();
    }

    public void OnExitState()
    {
    }
}