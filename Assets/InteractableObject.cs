using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : Interactable
{
    [HideInInspector]
    public Rigidbody rb;

    public UnityAction<InteractableObject> onInteractStart;
    public UnityAction<InteractableObject> onInteractEnd;

    public void StartInteract()
    {
        if(onInteractStart != null)
        {
            onInteractStart(this);
        }
    }

    public void StopInteract()
    {
        if (onInteractEnd != null)
        {
            onInteractEnd(this);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
