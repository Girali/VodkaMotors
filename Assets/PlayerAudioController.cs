using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField]
    private GameObject audioSource;
    private AudioSource[] audioSources;
    [SerializeField]
    private AudioClip[] steps;

    [SerializeField]
    private AudioClip landing;

    [SerializeField]
    private AudioClip jump;

    private void Awake()
    {
        audioSources = audioSource.GetComponents<AudioSource>();
    }

    private AudioSource FindFreeSource()
    {
        foreach (AudioSource item in audioSources)
        {
            if (!item.isPlaying)
                return item;
        }

        return null;
    }

    public void Landing()
    {
        if (Time.time > footStepTimer)
        {
            footStepTimer = Time.time + footStepDelay;
            AudioSource s = FindFreeSource();
            s.volume = 0.1f;
            s.pitch = Random.Range(0.9f, 1.1f);
            s.PlayOneShot(landing);
        }
    }

    public void Jump()
    {

        AudioSource s = FindFreeSource();
        s.volume = 0.1f;
        s.pitch = Random.Range(0.9f, 1.1f);
        s.PlayOneShot(jump);
    }

    public float footStepDelay = 0.25f;
    private float footStepTimer = 0;

    public void FootStep()
    {
        if (Time.time > footStepTimer)
        {
            footStepTimer = Time.time + footStepDelay; 
            AudioClip ac = steps[Random.Range(0, steps.Length)];
            AudioSource s = FindFreeSource();
            s.volume = 0.1f;
            s.pitch = Random.Range(0.9f, 1.1f);
            s.PlayOneShot(ac);
        }
    }
}
