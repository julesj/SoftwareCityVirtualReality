using UnityEngine;
using System.Collections;

public class SpotCalibrationSuccesshandler : MonoBehaviour {

    void Awake()
    {
        LifeCycle lifeCycle = FindObjectOfType<LifeCycle>();
        if (lifeCycle != null)
        {
            lifeCycle.OnInitHandler += OnInit;
        }
    }

    private void OnInit()
    {
        FindObjectOfType<TableCalibrator>().OnCalibrationCompleteHandler += OnCalibrationComplete;
    }

    private void OnCalibrationComplete(Vector3 center, float radius)
    {
        transform.position = new Vector3(center.x, 0, center.z);
    }
}
