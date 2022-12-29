using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    public AudioSource audioSource;
    private bool picked = false;

    public void OnTriggerEnter(Collider other)
    {
        if (!picked)
        {
            GameObject player = other.gameObject;

            if (player != null && player.tag == "Player")
            {
                picked = true;
                audioSource.Play();
                other.attachedRigidbody.GetComponent<PlayerItemController>().AddAmmo(4);
                Destroy(gameObject, 1);
            }
        }
    }
}
