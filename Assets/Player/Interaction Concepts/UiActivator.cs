using UnityEngine;
using System.Collections;

public class UiActivator : MonoBehaviour {

    public string interactionConceptName;

    void Awake()
    {
        EventBus.Register(this);
        gameObject.SetActive(false);
    }

    public void OnEvent(StartInteractionConceptEvent e)
    {
        if (e.newConcept.ToString().Equals(interactionConceptName))
        {
            gameObject.SetActive(true);
        }
    }

    public void OnEvent(StopInteractionConceptEvent e)
    {
        if (e.oldConcept.ToString().Equals(interactionConceptName))
        {
            gameObject.SetActive(false);
        }
    }

    void Update() { }
}
