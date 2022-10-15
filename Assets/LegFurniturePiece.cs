using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegFurniturePiece : FurniturePiece
{
    public Type currentType;

    public enum Type
    {
        Single,
        Double
    }
}
