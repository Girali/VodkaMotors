using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicManager : MonoBehaviour
{
    public AudioSource music;
    public float time;
    public float volume;

    private IEnumerator Start()
    {
        float i = 0;
        float target = time * 60f;

        while (target > i)
        {
            i++;
            music.volume = Mathf.Lerp(0, volume, i / target);
            yield return new WaitForEndOfFrame(); 
        }
    }
}
