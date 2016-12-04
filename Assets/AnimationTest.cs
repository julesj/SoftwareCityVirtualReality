using UnityEngine;
using System.Collections;

public class AnimationTest : MonoBehaviour {
    
	void Start () {
        AnimateThis anim = GetComponent<AnimateThis>();
        AnimateThis.Animation a = anim.Transformate(this)
               .ToPosition(new Vector3(0, 10, 0))
               .ToScale(new Vector3(10,2,3))
               .ToRotation(Quaternion.AngleAxis(90, Vector3.up))
               .Delay(1)
               .Duration(2)
               .Ease(AnimateThis.EaseSmooth)
            .Start();
	}
	
}
