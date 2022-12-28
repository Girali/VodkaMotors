using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    [SerializeField]
    private bool listen = false;

    [SerializeField]
    private PlayerMotor playerMotor;
    [SerializeField]
    private PlayerAudioController playerAudioController;

    public void FootStep()
    {
        if (listen) 
        {
            playerAudioController.FootStep();
        }
    }

    public void AnimEnd()
    {
        if (listen)
        {
            playerMotor.StartUse();
            playerMotor.GetComponent<PlayerFootController>().StartUse();
        }
    }
}
