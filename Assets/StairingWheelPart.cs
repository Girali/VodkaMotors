using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairingWheelPart : InteractablePart
{
    public Transform render;
    public float angleMax = 45;

    public void UpdateView(float i)
    {
        render.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(angleMax , -angleMax, i));
    }
}
