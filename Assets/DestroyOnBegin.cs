using UnityEngine;
using System.Collections;

public class DestroyOnBegin : MonoBehaviour {

    void Awake()
    {
        LifeCycle lifeCycle = FindObjectOfType<LifeCycle>();
        lifeCycle.OnBeginHandler += OnBegin;
    }

    private void OnBegin()
    {
        LifeCycle lifeCycle = FindObjectOfType<LifeCycle>();
        lifeCycle.OnBeginHandler -= OnBegin;
        GameObject.Destroy(gameObject);
    }
}
