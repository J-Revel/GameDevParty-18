using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource source;
    public bool playOnStart = true;

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = clips[Random.Range(0, clips.Length)];
        if(playOnStart)
            source.Play();
    }

    void Update()
    {
        
    }
}
