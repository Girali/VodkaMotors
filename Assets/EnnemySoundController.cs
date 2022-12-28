using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySoundController : MonoBehaviour
{
    public GameObject source;
    private AudioSource[] audioSources;
    public AudioClip[] hits;
    private float lastHit;

    public AudioClip[] steps;
    private float lastStep;

    private void Awake()
    {
        audioSources = source.GetComponents<AudioSource>();
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

    public void Hit()
    {
        if (Time.time > lastHit)
        {
            AudioSource s = FindFreeSource();
            AudioClip c = hits[Random.Range(0, hits.Length)];
            s.pitch = Random.Range(0.9f, 1.1f);
            s.PlayOneShot(c);
            lastHit = Time.time + 1f;
        }
    }

    public void FootStep()
    {
        if (Time.time > lastStep)
        {
            AudioSource s = FindFreeSource();
            AudioClip c = steps[Random.Range(0, hits.Length)];
            s.pitch = Random.Range(0.9f, 1.1f);
            s.PlayOneShot(c);
            lastStep = Time.time + 0.25f;
        }
    }
}
