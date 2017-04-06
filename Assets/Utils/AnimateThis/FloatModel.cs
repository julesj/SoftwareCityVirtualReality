using UnityEngine;
using System.Collections;

public class FloatModel : MonoBehaviour {

    public string name;

    [SerializeField]
    private float value;

    public delegate void OnFloatModelValueChanged(FloatModel model, bool scaleRotateAboutUser = false);
    public OnFloatModelValueChanged onFloatModelValueChangedHandler;

    public void SetValue(float value, bool scaleRotateAboutUser = false, bool sendEvent = true)
    {
        this.value = Mathf.Max(0, Mathf.Min(1, value));
        if (sendEvent)
        {
            onFloatModelValueChangedHandler(this, scaleRotateAboutUser);
        }
        Debug.Log("value: " + this.value);
    }

    public float GetValue()
    {
        return value;
    }
}
