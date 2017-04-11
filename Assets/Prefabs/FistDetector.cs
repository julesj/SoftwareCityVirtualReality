using UnityEngine;
using UnityEngine.Events;
using Leap;
using Leap.Unity;
using Leap.Unity.Attributes;
using System.Collections;
using System.Collections.Generic;


public class FistDetector : Detector
{
    public float Period = .1f; //seconds

    [Range(0,1)]
    public float OnValue = 0.9f;
    [Range(0,1)]
    public float OffValue = 0.8f;

    [AutoFind(AutoFindLocations.Parents)]
    [Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
    public IHandModel HandModel = null;

    private float gizmoSize = .1f;
    private IEnumerator watcherCoroutine;
    private Controller controller;

    void Awake()
    {
        watcherCoroutine = watcher();
    }

    private void Start()
    {
        controller = new Controller();
    }

    void OnEnable()
    {
        StopCoroutine(watcherCoroutine);
        StartCoroutine(watcherCoroutine);
    }

    void OnDisable()
    {
        StopCoroutine(watcherCoroutine);
    }

    IEnumerator watcher()
    {
        Hand useHand;
        while (true)
        {
            if (HandModel != null)
            {
                //Your logic to compute or check the current watchedValue goes here
                useHand = HandModel.GetLeapHand();
                if(useHand != null)
                {
                    if (useHand.GrabStrength > OnValue)
                    {
                        Activate();
                    }
                    if (useHand.GrabStrength < OffValue)
                    {
                        Deactivate();
                    }
                }            }
            yield return new WaitForSeconds(Period);
        }
    }
}