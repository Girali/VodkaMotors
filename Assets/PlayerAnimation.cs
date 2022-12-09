using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Vector2 currentRunValue;
    private float lerpTime = 0.1f;
    [SerializeField]
    private Animator an;

    public void Fall(bool b)
    {
        an.SetBool("Falling", b);
    }

    public void Driving(bool b)
    {
        an.SetBool("Driving", b);
    }

    public void Run(Vector2 v)
    {
        currentRunValue = Vector2.Lerp(currentRunValue, v, lerpTime);
        an.SetFloat("Z", currentRunValue.y);
        an.SetFloat("X", currentRunValue.x);
    }
}
