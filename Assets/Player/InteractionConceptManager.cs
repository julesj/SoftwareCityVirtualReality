using UnityEngine;
using System.Collections;

public enum InteractionConcept { Calibration, ScaleTranslate, Selection, Nothing };

public class InteractionConceptManager : MonoBehaviour {

    private InteractionConcept currentConcept = InteractionConcept.Nothing;

    void Awake()
    {
        EventBus.Register(this);
    }

    public void OnEvent(ChangeInteractionConceptEvent newConcept)
    {
        EventBus.Post(new StopInteractionConceptEvent(currentConcept));
        EventBus.Post(new StartInteractionConceptEvent(newConcept.newConcept));
        currentConcept = newConcept.newConcept;
    }
}

public interface InteractionConceptElement
{
    void ActivateElement();
    void DeactivateElement();
    InteractionConcept GetConcept();
}

public class ChangeInteractionConceptEvent
{
    public InteractionConcept newConcept;
    public ChangeInteractionConceptEvent(InteractionConcept newConcept)
    {
        this.newConcept = newConcept;
    }
}

public class StartInteractionConceptEvent
{
    public InteractionConcept newConcept;
    public StartInteractionConceptEvent(InteractionConcept newConcept)
    {
        this.newConcept = newConcept;
    }
}

public class StopInteractionConceptEvent
{
    public InteractionConcept oldConcept;
    public StopInteractionConceptEvent(InteractionConcept oldConcept)
    {
        this.oldConcept = oldConcept;
    }
}