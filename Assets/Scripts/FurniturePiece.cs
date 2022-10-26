using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePiece : MonoBehaviour
{
    public FurnitureType type;
    public AnchorType anchorType;
    public Rigidbody rb;

    public bool IsGrabed    {        get        {            return grabed;        }    }

    [SerializeField]
    protected Link[] links;
    protected List<LinkSet> linkSets;
    protected bool grabed = false;

    public void AddSet(Link f, Link t, Linker l)
    {
        linkSets.Add(new LinkSet(f, t, l));
    }

    public void RemoveSet(Linker l)
    {
        linkSets.RemoveAll((h) => h.IsEqual(l));
    }

    public LinkSet FindSetByFromLink(Link l)
    {
        return linkSets.Find((h) => h.IsEqual(l));
    }

    public class LinkSet
    {
        public Link from;
        public Link to;
        public Linker linker;

        public LinkSet(Link from, Link to, Linker linker)
        {
            this.from = from;
            this.to = to;
            this.linker = linker;
        }

        public bool IsEqual(Link f)
        {
            return f == from;
        }

        public bool IsEqual(Linker l)
        {
            return linker == l;
        }
    }

    public void AddRigidbody()
    {
        Collider c = GetComponent<Collider>();

        if (c.attachedRigidbody == null)
        {
            gameObject.AddComponent<Rigidbody>();
            rb = GetComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }



    public virtual void DetachePiece(Vector3 v)
    {
        StartCoroutine(CRT_Detach(v));
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

        if (li != null)
        {
            li.From.transform.parent = null;
            li.To.transform.parent = null;

            yield return new WaitForFixedUpdate();

            li.From.AddRigidbody();
            li.To.AddRigidbody();

            li.Detache(this);
        }
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
        linkSets = new List<LinkSet>();
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

    public void PlaceComponent(Link where, Link moving)
    {
        where.Place(moving);
        Destroy(moving.From.rb);
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
