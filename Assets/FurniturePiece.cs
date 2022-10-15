using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePiece : MonoBehaviour
{
    public FurnitureType type;
    public AnchorType anchorType;
    protected Rigidbody rb;

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



    public void PlaceComponent(Link f, Link other)
    {
        Destroy(rb);
        f.Place(other);
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
    Handrest
}
