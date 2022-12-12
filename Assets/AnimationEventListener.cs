using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    [SerializeField]
    private bool listen = false;

    [SerializeField]
    private PlayerMotor playerMotor;

    public void AnimEnd()
    {
        if(listen)
            playerMotor.StartUse();
    }
}
