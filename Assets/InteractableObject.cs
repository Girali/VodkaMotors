using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : Interactable
{
    [HideInInspector]
    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
