using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    [SerializeField]
    private Transform anchor;
    [SerializeField]
    private FurniturePiece from;
    private FurniturePiece to;

    public void Place(Link f)
    {
        f.from.transform.rotation *= f.anchor.rotation * Quaternion.Inverse(anchor.rotation);

        f.from.transform.position = anchor.position;
        f.from.transform.position += anchor.position - f.anchor.position;

        f.to = from;
        to = f.from;

        f.from.transform.parent = from.transform;
    }
}
