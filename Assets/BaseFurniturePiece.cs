using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFurniturePiece : FurniturePiece
{
    [SerializeField]
    private AnchorType backType;
    [SerializeField]
    private AnchorType legType;


    public override List<FurnitureSet> Init()
    {
        List <FurnitureSet> l = base.Init();

        l.Add(new FurnitureSet(FurnitureType.Back, backType));
        l.Add(new FurnitureSet(FurnitureType.Leg, legType));

        return l;
    }
}