using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartVideo : MonoBehaviour {
    private MovieTexture movie;
    public bool loop = true;

    private void Start()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        movie = (MovieTexture)rend.material.mainTexture;
        if (movie)
        {
            movie.Play();
            movie.loop = loop;
        }
    }

    private void OnEnable()
    {
        if (movie)
        {
            movie.Play();
        }
    }

    private void OnDisable()
    {
        movie.Stop();
    }
}
