using UnityEngine;
using System.Collections;

public class FloatModel : MonoBehaviour {

    public string name;

    [SerializeField]
    private float value;

    public delegate void OnFloatModelValueChanged(FloatModel model, bool scaleRotateAboutUser = false);
    public OnFloatModelValueChanged onFloatModelValueChangedHandler;

    public void SetValue(float value, bool scaleRotateAboutUser = false)
    {
        this.value = Mathf.Max(0, Mathf.Min(1, value));
        onFloatModelValueChangedHandler(this, scaleRotateAboutUser);
    }

    public float GetValue()
    {
        return value;
    }
}
