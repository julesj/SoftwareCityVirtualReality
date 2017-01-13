using UnityEngine;
using System.Collections;

public class SpotCalibrationSuccesshandler : MonoBehaviour {

    void Awake()
    {
        EventBus.Register(this);
    }

    public void OnEvent(SceneReadyEvent e)
    {
        TableCalibrator tableCalibrator = FindObjectOfType<TableCalibrator>();
        if (tableCalibrator != null)
        {
            tableCalibrator.OnCalibrationCompleteHandler += OnCalibrationComplete;
        }
    }

    private void OnCalibrationComplete(Vector3 center, float radius)
    {
        transform.position = new Vector3(center.x, 0, center.z);
    }
}
