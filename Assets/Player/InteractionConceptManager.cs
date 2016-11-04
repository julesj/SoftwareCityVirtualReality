using UnityEngine;
using System.Collections;

public enum InteractionConcept { Calibration, ScaleTranslate, Nothing };

public class InteractionConceptManager : MonoBehaviour {

    private InteractionConcept currentConcept = InteractionConcept.Nothing;

	// Use this for initialization
	void Start () {
	       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeConcept(InteractionConcept interactionConcept)
    {
        foreach (InteractionConceptElement element in GetComponentsInChildren<InteractionConceptElement>(true))
        {
            if (element.GetConcept() == interactionConcept)
            {
                if (currentConcept != element.GetConcept())
                {
                    element.ActivateElement();
                }
            }
            else
            {
                if (currentConcept == element.GetConcept())
                {
                    element.DeactivateElement();
                }
            }
        }
        currentConcept = interactionConcept;
    }
}

public interface InteractionConceptElement
{
    void ActivateElement();
    void DeactivateElement();
    InteractionConcept GetConcept();
}
