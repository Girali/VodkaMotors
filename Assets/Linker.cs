using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linker : FurniturePiece
{
    private Link link;
    private FurniturePiece from;

    public bool IsLinked
    {
        get
        {
            return link != null;
        }
    }


    public FurniturePiece From { get => from; }
    public Link Link { get => link; }

    public void PlacePiece(FurniturePiece f, Link l)
    {
        PlayerController.Instance.ReleaseCommand();
        f.PlaceComponent(link,l);
    }

    public bool PlaceLinker(Link l)
    {
        bool b = l.InsertLinker(this);

        if (b)
        {
            Destroy(rb);
            link = l;
            PlayerController.Instance.ReleaseCommand();
        }
        else
        {

        }

        return b;
    }
}
