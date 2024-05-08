using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpSoundEmittere : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource[] _audioSources;
    public ImpMovement ImpMovementScript;
    public float minInterval = 3f; // Minimum time between sound emissions
    public float maxInterval = 7f; // Maximum time between sound emissions

    private float nextEmissionTime;
    void Start()
    {
        _audioSources = ImpMovementScript.ImpSounds;
        nextEmissionTime = Time.time + Random.Range(minInterval, maxInterval);
    }

    void Update()
    {
        // Check if it's time to emit a sound
        if (Time.time >= nextEmissionTime)
        {
            EmitRandomSound();

            // Set next emission time
            nextEmissionTime = Time.time + Random.Range(minInterval, maxInterval);
        }
    }

    void EmitRandomSound()
    {
        // Select a random AudioSource from the array
        int randomIndex = Random.Range(0, _audioSources.Length);
        AudioSource selectedSource = _audioSources[randomIndex];

        // Check if the selected AudioSource is not null and is not already playing
        if (selectedSource != null && !selectedSource.isPlaying)
        {
            // Play the selected AudioSource
            selectedSource.Play();
        }
    }
}
