using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public virtual void Motor()
    {

    }

    public UnityAction<Interactable> onInteractStart;
    public UnityAction<Interactable> onInteractEnd;

    public UnityEvent onInteract;

    public string text;

    public void StartInteract()
    {
        if (onInteractStart != null)
        {
            onInteractStart(this);
        }

        onInteract.Invoke();
    }

    public void StopInteract()
    {
        if (onInteractEnd != null)
        {
            onInteractEnd(this);
        }
    }
}
