using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Engine", menuName = "Parts/Engine")]
public class EnginePartObject : VehiculPartObject
{
    public AudioClip clip;
    public float maxSpeed;
    public Vector2 pitchLimit;
    public Vector2 volumeLimit;
}
