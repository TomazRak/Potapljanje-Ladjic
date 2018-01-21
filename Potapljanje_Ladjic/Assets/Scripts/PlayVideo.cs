using UnityEngine;
using System.Collections;
using UnityEngine.UI;



[RequireComponent(typeof(AudioSource))]

public class PlayVideo : MonoBehaviour
{

    public MovieTexture movie;
    public MovieTexture movie1;
    public MovieTexture movie2;
    private AudioSource audio;

    // Use this for initialization
    void Start()
    {
        GetComponent<RawImage>().texture = movie as MovieTexture;
        audio = GetComponent<AudioSource>();
        audio.clip = movie.audioClip;
        int moja;
        moja = Random.Range(1, 3);
        if (moja == 1)
        {
            movie.Play();
        }
        if (moja == 2)
        {
            movie1.Play();
        }
        if (moja == 3)
        {
            movie2.Play();
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
