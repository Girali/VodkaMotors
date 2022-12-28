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

    public void Drink()
    {
        modelAnim.SetTrigger("Drink");
        sahdowAnim.SetTrigger("Drink");
    }

    public void Waking()
    {
        modelAnim.SetTrigger("Waking");
        sahdowAnim.SetTrigger("Waking");
    }

    public void Stance(int i)
    {
        modelAnim.SetInteger("Stance", i);
        sahdowAnim.SetInteger("Stance", i);
    }

    public void Fire()
    {
        modelAnim.SetTrigger("Fire");
        sahdowAnim.SetTrigger("Fire");
    }

    public void Reload()
    {
        modelAnim.SetTrigger("Reload");
        sahdowAnim.SetTrigger("Reload");
    }

    public void Aim(float p)
    {
        p = 1 - ((p + 90) / 180f);
        modelAnim.SetFloat("Pitch", p);
        sahdowAnim.SetFloat("Pitch", p);
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
