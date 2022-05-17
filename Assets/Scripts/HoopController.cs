using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopController : MonoBehaviour
{
    private ParticleSystem hoopParticleSystem;
    private AudioSource hoopAudioSource;

    void Start()
    {
        hoopParticleSystem = GetComponent<ParticleSystem>();
        hoopAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        hoopAudioSource.Play();
        hoopParticleSystem.Play();
    }
}
