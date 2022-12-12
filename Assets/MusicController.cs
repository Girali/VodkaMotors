using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public int startClip;
    public AudioClip[] clips;
    private AudioSource audioSource;
    public float maxVolume = 1f;
    private float lerpTime = 10f;
    private int lastClip;

    private Coroutine crt;

    private void Start()
    {
        audioSource =GetComponent<AudioSource>();
        crt = StartCoroutine(CRT_Music(startClip));
    }

    public void StartNewMusic()
    {
        if(crt != null)
        {
            StopCoroutine(crt);
        }

        int i = Random.Range(0, clips.Length);

        while (i == lastClip)
        {
            i = Random.Range(0, clips.Length);
        }

        crt = StartCoroutine(CRT_Music(i));
    }

    IEnumerator CRT_Music(int i)
    {

        lastClip = i;
        audioSource.clip = clips[i];
        audioSource.Play();
        audioSource.volume = 0;

        float endWaitTimer = Time.time + clips[i].length - lerpTime;

        float t = 0;

        while (t < lerpTime)
        {
            t += Time.deltaTime;
            audioSource.volume = (t / lerpTime) * maxVolume;
            yield return new WaitForEndOfFrame();
        }

        audioSource.volume = maxVolume;

        yield return new WaitUntil(() => endWaitTimer < Time.time);

        t = lerpTime;

        while (t > 0)
        {
            t -= Time.deltaTime;
            audioSource.volume = (t / lerpTime) * maxVolume;
            yield return new WaitForEndOfFrame();
        }

        audioSource.volume = 0;

        StartNewMusic();
    }
}
