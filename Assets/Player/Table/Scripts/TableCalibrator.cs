using UnityEngine;
using System.Collections;
using VRTK;

public class TableCalibrator: MonoBehaviour {
    
    public delegate void OnCalibrationComplete(Vector3 center, float radius);
    public OnCalibrationComplete OnCalibrationCompleteHandler;

    public Vector3 lastCalibratedCenter;
    public float lastCalibratedRadius;

    private const uint maxCalibrationVectors = 1000;

    private Vector3[] appButtonPoints = new Vector3[maxCalibrationVectors];
    private Vector3[] directions = new Vector3[maxCalibrationVectors];
    private Vector3[] controllerBasePoints = new Vector3[maxCalibrationVectors];
    private uint numberOfCalibrationVectors = 0;

    public void AddCalibrationVector(VRTK_ControllerActions controllerActions)
    {
        controllerActions.TriggerHapticPulse(3999);

        Transform appButton = controllerActions.gameObject.transform.Find("Model/button/attach");
        Transform sysButton = controllerActions.gameObject.transform.Find("Model/sys_button/attach");
        Transform controllerBase = controllerActions.gameObject.transform.Find("Model/base/attach");

        Vector3 direction = sysButton.position - appButton.position;

        appButtonPoints[numberOfCalibrationVectors] = appButton.position;
        directions[numberOfCalibrationVectors] = direction;
        controllerBasePoints[numberOfCalibrationVectors] = controllerBase.position;

        if (numberOfCalibrationVectors < maxCalibrationVectors - 1)
        {
            numberOfCalibrationVectors++;
            Calibrate();
        }
        else
        {
            //CancelInvoke("addCalibrationVector");
        }
    }

    public void ResetCalibration()
    {
        appButtonPoints = new Vector3[maxCalibrationVectors];
        directions = new Vector3[maxCalibrationVectors];
        controllerBasePoints = new Vector3[maxCalibrationVectors];
        numberOfCalibrationVectors = 0;
    }

    private void Calibrate()
    {
        if (numberOfCalibrationVectors < 2)
        {
            return;
        }
        
        uint actualSize = numberOfCalibrationVectors;
        uint size = 0;

        Vector3 result = new Vector3();
        for (int i = 0; i<actualSize; i++)
        {
            for (int j = i+1; j < actualSize; j++)
            {
                Vector3 pointLine1;
                Vector3 pointLine2;
                ClosestPointsOnTwoLines(out pointLine1, out pointLine2, appButtonPoints[i], directions[i], appButtonPoints[j], directions[j]);
                result += pointLine1;
                result += pointLine2;
                size += 2;
            }
        }

        Vector3 center = result / size;

        float radiusResult = 0;
        float heightResult = 0;
        for (int i = 0; i < actualSize; i++)
        {
            radiusResult += Vector3.Distance(center, new Vector3(appButtonPoints[i].x, center.y, appButtonPoints[i].z));
            heightResult += controllerBasePoints[i].y;
        }

        float radius = radiusResult / actualSize;
        float height = heightResult / actualSize - 0.015f; // - constant value

        lastCalibratedCenter = new Vector3(center.x, height, center.z);
        lastCalibratedRadius = radius + 0.04f; // + constant value

        OnCalibrationCompleteHandler(lastCalibratedCenter, lastCalibratedRadius);
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
