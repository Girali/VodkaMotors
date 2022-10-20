using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandrestFurniturePiece : FurniturePiece
{
    public Type currentType;
    public Transform mirror;

    public override List<FurnitureSet> Init()
    {
        List<FurnitureSet> l = base.Init();
        switch (currentType)
        {
            case Type.Both:
                break;
            case Type.Left:
                mirror.localScale = new Vector3(-1, 1, 1);
                break;
            case Type.Right:
                break;
            default:
                break;
        }

        return l;
    }

    public enum Type
    {
        Both,
        Left,
        Right
    }
}