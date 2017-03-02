using UnityEngine;
using System.Collections;

public class ScaleAnimator : MonoBehaviour {

    public Vector3 toPosition;
    public Vector3 toScale;
    public float speed = 3.75f;

    private Vector3 fromPosition;
    private Vector3 fromScale;

    private Vector3 actualPosition;
    private Vector3 actualScale;

    void Start () {
        fromPosition = transform.localPosition;
        fromScale = transform.localScale;

        actualPosition = fromPosition;
        actualScale = fromScale;
    }
	
	void Update () {
        transform.localPosition = Vector3.Lerp(transform.localPosition, actualPosition, Time.deltaTime * speed);
        transform.localScale = Vector3.Lerp(transform.localScale, actualScale, Time.deltaTime * speed);
    }

    public void ResetAnimation()
    {
        transform.localPosition = fromPosition;
        transform.localScale = fromScale;

        actualPosition = fromPosition;
        actualScale = fromScale;
    }

    public void SetAnimationProgress(float progress)
    {
        float xP = (toPosition.x - fromPosition.x) * progress;
        float yP = (toPosition.y - fromPosition.y) * progress;
        float zP = (toPosition.z - fromPosition.z) * progress;

        actualPosition = new Vector3(xP, yP, zP);

        float xS = (toScale.x - fromScale.x) * progress;
        float yS = (toScale.y - fromScale.y) * progress;
        float zS = (toScale.z - fromScale.z) * progress;

        actualScale = new Vector3(xS, yS, zS);
    }
}
