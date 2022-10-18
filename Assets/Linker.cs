using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linker : FurniturePiece
{
    private Link link;
    private FurniturePiece from;

    private bool isUsed = false;


    public bool IsUsed
    {
        get
        {
            return isUsed;
        }
    }

    public bool IsLinked
    {
        get
        {
            return link != null;
        }
    }


    public FurniturePiece From { get => from; }
    public Link Link { get => link; }

    public void PlacePieceInverse(FurniturePiece f, Link l)
    {
        PlayerController.Instance.ReleaseCommand();
        f.PlaceComponentInverse(link, l);
        isUsed = true;
    }

    public override void DetachePiece(Vector3 v)
    {
        if (transform.parent != null)
        {
            gameObject.AddComponent<Rigidbody>();
            rb = GetComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            transform.parent = null;

            AddForceNextPhysFrame(transform.position, transform.forward * 200f);
            isUsed = false;
            gameObject.layer = 8;

            if (link)
            {
                SFX_Controller.Instance.CreateSFX(SFX_Type.PopOut, transform.position, transform.forward);
                link.InsertLinker(null);
                link.gameObject.SetActive(true);
                link = null;
            }
        }
    }

    public void PlacePiece(FurniturePiece f, Link l)
    {
        PlayerController.Instance.ReleaseCommand();
        f.PlaceComponent(link, l);
        isUsed = true;
    }

    public void ResetLink()
    {
        isUsed = false;
    }

    public bool PlaceLinker(Link l)
    {
        bool b = l.InsertLinker(this);

        if (b)
        {
            Destroy(rb);
            gameObject.layer = 7;
            link = l;
            PlayerController.Instance.ReleaseCommand();
        }
        else
        {

        }

        return b;
    }
}
