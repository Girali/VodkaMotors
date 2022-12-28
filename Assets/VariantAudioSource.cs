using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariantAudioSource : MonoBehaviour
{
    public float volume = 0.2f;
    public Vector2 ptuch = new Vector2(0.85f, 1.15f);

    private void Awake()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.pitch = Random.Range(ptuch.x, ptuch.y);
        audioSource.Play();
    }
}
