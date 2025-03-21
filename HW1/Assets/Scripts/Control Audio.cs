using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float startTime = 1f;

    private bool isPaused = false;

    void Start()
    {
        audioSource.time = startTime;
        audioSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                isPaused = true;
            }
            else if (isPaused)
            {
                audioSource.UnPause();
                isPaused = false;
            }
            else
            {
                audioSource.time = startTime;
                audioSource.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            audioSource.Stop();
            isPaused = false;
        }
    }
}
