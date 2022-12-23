using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public int startClip;
    public AudioClip[] clips;
    public float[] bpm;
    public AudioSource audioSourceMusic;
    public AudioSource audioSourceDialogue;
    public float maxVolume = 1f;
    private float lerpTime = 10f;
    private int lastClip;
    public TMP_Text text;
    private Coroutine crt;

    public ParticleSystem[] systems;

    private AudioProcessor audioProcessor;
    private float nextBeat;

    private bool playBeat = true;

    private void Start()
    {
        audioProcessor = GetComponent<AudioProcessor>();
        crt = StartCoroutine(CRT_Music(startClip));
    }

    private void FixedUpdate()
    {
        if (playBeat)
        {
            if (Time.time > nextBeat)
            {
                nextBeat = Time.time + (1f / (bpm[lastClip] / 60f));
                OnBeat();
            }
        }
    }

    public void OnBeat()
    {
        foreach (ParticleSystem s in systems)
        {
            s.Clear(true);
            s.Play(true);
        }
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

    public void PlayDiaolgue(AudioClip clip)
    {
        StartCoroutine(CRT_Diaoluge(clip));
    }

    private IEnumerator CRT_Diaoluge(AudioClip clip)
    {
        audioSourceDialogue.clip = clip;
        audioSourceDialogue.Play();

        playBeat = false;

        float startVolume = audioSourceMusic.volume;

        for (int i = 0; i < 60; i++)
        {
            audioSourceDialogue.volume = Mathf.Lerp(0, 0.8f, i / 60f);
            audioSourceMusic.volume = Mathf.Lerp(startVolume, 0, i / 60f);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(clip.length - 2);

        playBeat = true;

        for (int i = 0; i < 60; i++)
        {
            audioSourceDialogue.volume = Mathf.Lerp(0.8f, 0, i / 60f);
            audioSourceMusic.volume = Mathf.Lerp(0, startVolume, i / 60f);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CRT_Music(int i)
    {
        lastClip = i;
        audioSourceMusic.clip = clips[i];
        audioSourceMusic.Play();
        audioProcessor.StartSong(); 
        audioSourceMusic.volume = 0;
        nextBeat = Time.time + (1f / (bpm[i] / 60f));
        text.text = audioSourceMusic.clip.name;

        float endWaitTimer = Time.time + clips[i].length - lerpTime;

        float t = 0;

        while (t < lerpTime)
        {
            t += Time.deltaTime;
            audioSourceMusic.volume = (t / lerpTime) * maxVolume;
            yield return new WaitForEndOfFrame();
        }

        audioSourceMusic.volume = maxVolume;

        yield return new WaitUntil(() => endWaitTimer < Time.time);

        t = lerpTime;

        while (t > 0)
        {
            t -= Time.deltaTime;
            audioSourceMusic.volume = (t / lerpTime) * maxVolume;
            yield return new WaitForEndOfFrame();
        }

        audioSourceMusic.volume = 0;

        StartNewMusic();
    }
}
