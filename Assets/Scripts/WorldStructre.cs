using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStructre : MonoBehaviour
{
    public Type type;

    public enum Type
    {
        Major,
        Minor,
        Mission,
        Deatils
    }

    public float DistanceFromPlayer()
    {
        return Vector3.Distance(transform.position, GameController.Instance.player.transform.position);
    }

    public virtual void Init()
    {
        if(type == Type.Major)
            GameController.Instance.AddMajorStruct(this);
    }
}
