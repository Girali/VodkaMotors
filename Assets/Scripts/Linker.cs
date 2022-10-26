using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linker : FurniturePiece
{
    private Link link;
    private Link toLink;
    private bool isUsed = false;

    public bool IsUsed { get { return isUsed; } }
    public bool IsLinked { get { return link != null; } }
    public Link Link { get => link; }
    public Link ToLink { get => toLink; set => toLink = value; }

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
                toLink = null;
            }
        }
    }

    public void ResetLink()
    {
        isUsed = false;
    }

    public void PlaceLinker(Link l)
    {
        l.InsertLinker(this);
        Destroy(rb);
        gameObject.layer = 7;
        link = l;
        PlayerController.Instance.ReleaseCommand();
    }
}
