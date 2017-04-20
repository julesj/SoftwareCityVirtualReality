using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVideosConsecutiv : MonoBehaviour {

    private MovieTexture movie;
    public GameObject first;
    public GameObject second;

    private void Start()
    {
        Renderer rend = first.gameObject.GetComponent<Renderer>();
        movie = (MovieTexture)rend.material.mainTexture;
        second.SetActive(false);
    }

    private void Update()
    {
        if (!movie.isPlaying)
        {
            ChangeVideo();
        }
    }

    private void ChangeVideo()
    {
        MovieTexture firstTexture = (MovieTexture) first.gameObject.GetComponent<Renderer>().material.mainTexture;
        MovieTexture secondTexture = (MovieTexture) second.gameObject.GetComponent<Renderer>().material.mainTexture;
        if (movie == firstTexture)
        {
            first.SetActive(false);
            movie = secondTexture;
            second.SetActive(true);
        } else if (movie == secondTexture)
        {
            second.SetActive(false);
            movie = firstTexture;
            first.SetActive(true);
        }
    }
}
