using UnityEngine;
using System;
using System.Collections;

interface ICircleMeasurement
{
    event EventHandler<CircleMeasureCompletedEventArgs> CircleMeasureCompleted;
}

public class CircleMeasureController : MonoBehaviour, ICircleMeasurement
{
    public event EventHandler<CircleMeasureCompletedEventArgs> CircleMeasureCompleted;

    private bool isMeasuring = false;

    void Start () {
	    
	}
	

	void Update () {
	    if (isMeasuring)
        {
            CircleMeasureCompletedEventArgs args = new CircleMeasureCompletedEventArgs();
            args.radius = 1.0f;
            args.center = Vector3.zero;

            OnCircleMeasureCompleted(args);
        }
	}

    protected virtual void OnCircleMeasureCompleted(CircleMeasureCompletedEventArgs e)
    {
        EventHandler<CircleMeasureCompletedEventArgs> handler = CircleMeasureCompleted;

        if (handler != null)
        {
            handler(this, e);
        }
    }
}


public class CircleMeasureCompletedEventArgs : EventArgs
{
    public float radius;
    public Vector3 center;
}