using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSources;
    void Start()
    {
        this.audioSources = GetComponentsInChildren<AudioSource>() ;
    }

    public void ControlRadio()
    {
        foreach(AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            else
            { 
                audioSource.Play();
            } 
        }
    }
}
