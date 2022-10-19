using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePiece : MonoBehaviour
{
    public FurnitureType type;
    public AnchorType anchorType;
    public Rigidbody rb;

    public bool IsGrabed
    {
        get
        {
            return grabed;
        }
    }

    [SerializeField]
    protected Link[] links;
    protected bool grabed = false;

    public void AddRigidbody()
    {
        Collider c = GetComponent<Collider>();

        if (c.attachedRigidbody == null)
        {
            Debug.Log(name);
            gameObject.AddComponent<Rigidbody>();
            rb = GetComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    IEnumerator CRT_Detach(Vector3 v)
    {
        Link li = null;
        float d = float.MaxValue;

        foreach (Link l in links)
        {
            if (d > Vector3.Distance(l.Anchor.position, v))
            {
                if (l.To != null)
                {
                    li = l;
                    d = Vector3.Distance(l.Anchor.position, v);
                }
            }
        }

        li.From.transform.parent = null;
        li.To.transform.parent = null;

        yield return new WaitForFixedUpdate();

        li.From.AddRigidbody();
        li.To.AddRigidbody();

        li.Detache(this);
    }

    public virtual void DetachePiece(Vector3 v)
    {
        StartCoroutine(CRT_Detach(v));
    }

    public void CheckLinkState()
    {

    }

    public Link GetOtherLink(FurniturePiece f)
    {
        foreach (Link l in links)
        {
            if (l.To == f)
                return l;
        }

        return null;
    }

    public void AddForceNextPhysFrame(Vector3 p, Vector3 f)
    {
        StartCoroutine(CRT_AddForceNextPhysFrame(p, f));
    }

    IEnumerator CRT_AddForceNextPhysFrame(Vector3 p, Vector3 f)
    {
        yield return new WaitForFixedUpdate();
        rb.AddForceAtPosition(f, p);
    }

    public virtual List<FurnitureSet> Init()
    {
        List<FurnitureSet> needs = new List<FurnitureSet>();
        return needs;
    }

    public virtual void StartGrab()
    {
        grabed = true;
        foreach (Link l in links)
        {
            l.StartGrab();
        }
    }

    public virtual void StopGrab()
    {
        grabed = false;
        foreach (Link l in links)
        {
            l.StopGrab();
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PlaceComponentInverse(Link f, Link on)
    {
        f.PlaceInverse(on);
        Destroy(f.From.rb);
    }

    public void PlaceComponent(Link f, Link on)
    {
        f.Place(on);
        Destroy(rb);
    }
}

public class FurnitureSet
{
    public FurnitureType type;
    public AnchorType anchorType;

    public FurnitureSet(FurnitureType t, AnchorType tt)
    {
        type = t;
        anchorType = tt;
    }

}

public enum AnchorType
{
    None,
    Simple,
    Double
}

public enum FurnitureType
{
    Linker,
    Base,
    Leg,
    Back,
    Handrest,
    Tool
}
