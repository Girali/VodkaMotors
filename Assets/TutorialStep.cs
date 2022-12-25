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
    }

    public virtual void End()
    {
    }
}
