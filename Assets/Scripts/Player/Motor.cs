using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    protected Rigidbody rb;
    protected Collider cldr;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        cldr = GetComponent<Collider>();
    }

    public virtual void Move(bool forward, bool backward, bool left, bool right, bool jump, bool sprint, float yaw, float pitch, RaycastHit hit, bool interact)
    {

    }
}
