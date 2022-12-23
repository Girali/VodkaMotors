using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePart : InteractableObject
{
    public VehiculPartObject vehiculPartObject;
    public bool disableCollidersOnStop = false;
    public Collider[] collisionColliders;
    public bool collectablePart = false;
    protected VehiculMotor vehiculMotor;

    public virtual void Init(VehiculMotor vm)
    {
        vehiculMotor = vm;
    }

    public virtual void StartInteraction()
    {
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            if (disableCollidersOnStop)
            {
                foreach (Collider c in collisionColliders)
                {
                    c.enabled = true;
                }
            }
        }
    }

    public virtual void StopInteraction()
    {
        Destroy(rb);

        if (disableCollidersOnStop)
        {
            foreach (Collider c in collisionColliders)
            {
                c.enabled = false;
            }
        }
    }
}
