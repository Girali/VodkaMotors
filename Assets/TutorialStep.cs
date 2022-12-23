using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialStep : MonoBehaviour
{
    public UnityEvent onStart;
    public UnityEvent onEnd;

    public virtual void Begin()
    {
        Debug.LogError(name);
    }

    public virtual void End()
    {
        Debug.LogWarning(name);
    }
}
