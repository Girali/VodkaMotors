using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoorOpener : MonoBehaviour
{
    [SerializeField]
    private Transform door1;

    [SerializeField]
    private Transform doorOpen1;
    [SerializeField]
    private Transform doorClose1;

    [SerializeField]
    private Transform door2;

    [SerializeField]
    private Transform doorOpen2; 
    [SerializeField]
    private Transform doorClose2;

    private bool isOpened = false;

    private void Update()
    {
        if (isOpened)
        {
            door1.position = Vector3.Lerp(door1.position, doorOpen1.position, 0.05f);
            door2.position = Vector3.Lerp(door2.position, doorOpen2.position, 0.05f);
        }
        else
        {
            door1.position = Vector3.Lerp(door1.position, doorClose1.position, 0.05f);
            door2.position = Vector3.Lerp(door2.position, doorClose2.position, 0.05f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject player = other.gameObject;

        if(player != null && player.tag == "Player")
        {
            isOpened = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject player = other.gameObject;

        if (player != null && player.tag == "Player")
        {
            isOpened = false;
        }
    }
}
