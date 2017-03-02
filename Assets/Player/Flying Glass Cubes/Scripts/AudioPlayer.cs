using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {

    public AudioClip[] clips;
    public float pitchRangeNegative = 0;
    public float pitchRangePositive = 0;


    private AudioSource source;

	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	public void Play()
    {
        if (source != null)
        {
            float pitchNegative = Mathf.Abs(pitchRangeNegative);

            source.pitch = 1 + (Random.value * (pitchNegative + pitchRangePositive)) - pitchNegative;
            source.clip = clips[(int)(Random.value * (clips.Length))];
            source.Play();
        }
    }
}
