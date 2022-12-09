using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Vector2 currentRunValue;
    private float lerpTime = 0.1f;
    [SerializeField]
    private Animator modelAnim;
    [SerializeField]
    private Animator sahdowAnim;

    public void Fall(bool b)
    {
        modelAnim.SetBool("Falling", b);
        sahdowAnim.SetBool("Falling", b);
    }

    public void Driving(bool b)
    {
        modelAnim.SetBool("Driving", b);
        sahdowAnim.SetBool("Driving", b);
    }

    public void Run(Vector2 v)
    {
        currentRunValue = Vector2.Lerp(currentRunValue, v, lerpTime);
        modelAnim.SetFloat("Z", currentRunValue.y);
        modelAnim.SetFloat("X", currentRunValue.x);
        sahdowAnim.SetFloat("Z", currentRunValue.y);
        sahdowAnim.SetFloat("X", currentRunValue.x);
    }
}
