using UnityEngine;
using System.Collections;

public class DecorationManager : MonoBehaviour {


    public Decoration[] prefabs;
    public int count;
    public float maxDistance;

    private Decoration[] instances;

	void Start () {
        instances = new Decoration[count];
        for(int i=0; i< count; i++)
        {
            instances[i] = Instantiate(prefabs[((int)Random.value * (instances.Length))]);
            Randomize(instances[i]);
        }
	}

    private void Randomize(Decoration decoration)
    {
        decoration.transform.position = transform.position + new Vector3(Random.value - 0.5f, 0.3f + Random.value / 2f, Random.value - 0.5f).normalized * maxDistance * Random.value;
        Rigidbody rb = decoration.GetComponentInChildren<Rigidbody>();
        rb.angularVelocity = new Vector3(Random.value, Random.value, Random.value);
        rb.maxAngularVelocity = Random.value * 5;
        rb.velocity = new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f);
    }

    void Update()
    {
        Vector3 thisPos = transform.position;
        for (int i = 0; i < count; i++)
        {
            Vector3 thatPos = instances[i].transform.position;
            if (thatPos.y < 0 || Vector3.Distance(thisPos, thatPos) > maxDistance)
            {
                Randomize(instances[i]);
            }
        }
    }
}
