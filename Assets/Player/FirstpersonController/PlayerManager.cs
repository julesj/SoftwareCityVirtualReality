using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public GameObject playerOVR;
	public GameObject playerFallback;

	void Awake () {
		bool ovrPresent = VRDevice.isPresent;
        GameObject player = (GameObject) GameObject.Instantiate(ovrPresent ? playerOVR : playerFallback, transform.position, transform.rotation);

        //GameObject prototypeUiNode = transform.Find("Prototype UI Node").gameObject;
        //GameObject cameraGameObject = player.gameObject.GetComponentInChildren<Camera>().gameObject;

        //Copy(prototypeUiNode, cameraGameObject);

        GameObject prototypeComtrollerNode = transform.Find("Prototype Controller Node").gameObject;
        GameObject controllerGameObject = player.gameObject.GetComponentInChildren<CharacterController>().gameObject;

        Copy(prototypeComtrollerNode, controllerGameObject);

        Destroy(gameObject);
	}

    private void Copy(GameObject from, GameObject to)
    {
        foreach (Component comp in from.GetComponents<Component>())
        {
            if (comp != null && !(comp is Camera || comp is Transform || comp is Rigidbody))
            {
                CopyComponent(comp, to);
            }
        }

        for (int i = 0; i < from.transform.childCount; i++)
        {
            GameObject clone = GameObject.Instantiate(from.transform.GetChild(i).gameObject);
            clone.transform.SetParent(to.transform, false);
        }
    }

    private Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        if (copy is Behaviour)
        {
            ((Behaviour)copy).enabled = ((Behaviour)original).enabled;
        }
        return copy;
    }
}
