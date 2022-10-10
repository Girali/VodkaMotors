using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePiece : MonoBehaviour
{
    [SerializeField]
    private Link[] links;
    [SerializeField]
    private Link anchor;

    public void PlaceComponent(Link f)
    {
        f.Place(anchor);
    }
}
