using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWheelPart : InteractablePart
{
    public WheelCollider wheel;

    public override void StopInteraction()
    {
        wheel.enabled = true;
        base.StopInteraction();
    }
}
