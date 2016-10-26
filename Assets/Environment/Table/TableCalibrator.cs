using UnityEngine;
using System.Collections;
using VRTK;

public class TableCalibrator : MonoBehaviour {

    public VRTK_ControllerActions controllerActions;

    private Transform appButton;
    private Transform sysButton;

    public Vector3 calibratedCenter;
    public float calibratedRadius;

    private const uint maxCalibrationVectors = 1000;
    private Vector3[] points = new Vector3[maxCalibrationVectors];
    private Vector3[] directions = new Vector3[maxCalibrationVectors];
    private uint numberOfCalibrationVectors = 0;

    private bool isCalibrating = false;
    private bool shouldCalibrate = false;

    void Start () {
        controllerActions.ControllerModelVisible += new ControllerActionsEventHandler(ControllerDidBecomeVisible);
        appButton = controllerActions.gameObject.transform.FindChild(controllerActions.modelElementPaths.appMenuModelPath + "/attach");
        sysButton = controllerActions.gameObject.transform.FindChild(controllerActions.modelElementPaths.systemMenuModelPath + "/attach");
    }
	
	void Update () {
        if (isCalibrating)
        {
            return;
        } else if (shouldCalibrate)
        {
            print("calibrating... x:" + calibratedCenter.x + " y:" + calibratedCenter.y + " z:" + calibratedCenter.z + " r:" + calibratedRadius);
            calibrate();
        }
    }

    public void StartCalibration()
    {
        InvokeRepeating("addCalibrationVector", 5, 0.5f);
        shouldCalibrate = true;
    }

    public void StopCalibration()
    {
        CancelInvoke();
        shouldCalibrate = false;
        print("calibration complete: x:" + calibratedCenter.x + " y:" + calibratedCenter.y + " z:" + calibratedCenter.z + " r:" + calibratedRadius);
    }


    private void calibrate()
    {
        if (numberOfCalibrationVectors < 6)
        {
            return;
        }

        isCalibrating = true;
        uint actualSize = numberOfCalibrationVectors;

        uint size = 0;
        Vector3 result = new Vector3();

        for (int i = 0; i<actualSize; i++)
        {
            for (int j = i+1; j < actualSize; j++)
            {
                Vector3 pointLine1;
                Vector3 pointLine2;
                ClosestPointsOnTwoLines(out pointLine1, out pointLine2, points[i], directions[i], points[j], directions[j]);
                result += pointLine1;
                result += pointLine2;
                size += 2;
            }
        }

        calibratedCenter = result / size;
        calibratedRadius = Vector3.Distance(calibratedCenter, sysButton.position);
        isCalibrating = false;
    }

    private void addCalibrationVector()
    {
        controllerActions.TriggerHapticPulse(3999);
        Vector3 direction = sysButton.position - appButton.position;
        points[numberOfCalibrationVectors] = appButton.position;
        directions[numberOfCalibrationVectors] = direction;
        if (numberOfCalibrationVectors < maxCalibrationVectors-1)
        {
            numberOfCalibrationVectors++;
        } else
        {
            CancelInvoke("addCalibrationVector");
        }
    }

    void ControllerDidBecomeVisible(object sender, ControllerActionsEventArgs e)
    { 
        print("Controller became visible");
    }

    //Two non-parallel lines which may or may not touch each other have a point on each line which are closest
    //to each other. This function finds those two points. If the lines are not parallel, the function 
    //outputs true, otherwise false.
    public static bool ClosestPointsOnTwoLines(out Vector3 closestPointLine1, out Vector3 closestPointLine2, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        closestPointLine1 = Vector3.zero;
        closestPointLine2 = Vector3.zero;

        float a = Vector3.Dot(lineVec1, lineVec1);
        float b = Vector3.Dot(lineVec1, lineVec2);
        float e = Vector3.Dot(lineVec2, lineVec2);

        float d = a * e - b * b;

        //lines are not parallel
        if (d != 0.0f)
        {

            Vector3 r = linePoint1 - linePoint2;
            float c = Vector3.Dot(lineVec1, r);
            float f = Vector3.Dot(lineVec2, r);

            float s = (b * f - c * e) / d;
            float t = (a * f - c * b) / d;

            closestPointLine1 = linePoint1 + lineVec1 * s;
            closestPointLine2 = linePoint2 + lineVec2 * t;

            return true;
        }

        else
        {
            return false;
        }
    }
}
