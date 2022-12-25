using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DialogueController : MonoBehaviour
{
    public Sequence[] sequences;

    private Sequence currentSequence;
    public ParticleSystem[] systems;

    public MusicController musicController;

    public UnityEvent onDialogueEnd;

    public void StartDiaoluge(string s)
    {
        currentSequence = sequences.First((sqc) => sqc.name == s);
        StartCoroutine(DiaolgueTime());
    }

    private IEnumerator DiaolgueTime()
    {
        musicController.PlayDiaolgue(currentSequence.clip);
        GUI_Controller.Instance.AddDialogue(currentSequence);
        foreach (var s in systems)
        {
            s.Play(true);
        }
        yield return new WaitForSeconds(currentSequence.clip.length);
        onDialogueEnd.Invoke();
        foreach (var s in systems)
        {
            s.Stop(true);
        }
    }

    [Serializable]
    public class Sequence
    {
        public AudioClip clip;
        public string name;
        [TextArea]
        public string text;
    }
}
