using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    protected Rigidbody rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Move(bool forward, bool backward, bool left, bool right, bool jump, bool sprint, float yaw, float pitch, RaycastHit hit)
    {

    }
}
