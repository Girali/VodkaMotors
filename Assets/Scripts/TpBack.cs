using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpBack : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FurniturePiece>())
            other.transform.position = target.position;
    }
}
