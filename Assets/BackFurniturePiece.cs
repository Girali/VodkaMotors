using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackFurniturePiece : FurniturePiece
{
    public Type currentType;

    [SerializeField]
    private Link handrestLeftLink;
    [SerializeField]
    private Link handrestRightLink;

    public override List<FurnitureSet> Init()
    {
        List<FurnitureSet> f = base.Init();

        switch (currentType)
        {
            case Type.NoHandrest:
                break;
            case Type.Handrest:
                f.Add(new FurnitureSet(FurnitureType.Handrest, AnchorType.Simple));
                break;
            default:
                break;
        }

        return f;
    }

    public enum Type
    {
        NoHandrest,
        Handrest
    }
}
