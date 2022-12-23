using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableEnginePart : InteractablePart
{
    public AudioSource audioSource;
    private EnginePartObject enginePartObject;

    public override void Init(VehiculMotor vm)
    {
        base.Init(vm);
        enginePartObject = (EnginePartObject)vehiculPartObject;
        audioSource.clip = enginePartObject.clip;
        audioSource.Play();
    }

    private void Update()
    {
        audioSource.pitch = Mathf.Lerp(enginePartObject.pitchLimit.x, enginePartObject.pitchLimit.y, vehiculMotor.Speed / enginePartObject.maxSpeed);
        audioSource.volume =  Mathf.Lerp(enginePartObject.volumeLimit.x, enginePartObject.volumeLimit.y, vehiculMotor.Speed / enginePartObject.maxSpeed);
    }
}
