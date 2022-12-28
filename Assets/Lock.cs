using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lock : MonoBehaviour
{
    public UnityEvent onShoot;

    public void Shoot()
    {
        gameObject.SetActive(false);
        onShoot.Invoke();
    }
}
