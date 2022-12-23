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

    protected bool shiftUp = false;
    protected bool shiftPressed = false;
    protected bool shiftDown = false;

    public virtual void Move(bool forward, bool backward, bool left, bool right, bool jump, bool sprint, float yaw, float pitch, RaycastHit hit, bool interact)
    {
        if(shiftPressed == false)
        {
            if (sprint == true)
            {
                if(shiftDown == true)
                {
                    shiftDown = false;
                    shiftPressed = true;
                }
                else
                {
                    shiftDown = true;
                }
            }
        }
        else
        {
            if (sprint == false)
            {
                if (shiftUp == true)
                {
                    shiftUp = false;
                }
                else
                {
                    shiftUp = true;
                    shiftPressed = false;
                }

            }
        }
    }
}
