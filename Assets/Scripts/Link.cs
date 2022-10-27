using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    [SerializeField]
    private Transform anchor;
    [SerializeField]
    private FurniturePiece from;
    private FurniturePiece to;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Link[] neededLinks;
    private bool glueIn = false;
    private Linker linker = null;
    private Linker linkInZone = null;
    private bool grabed = false;
    private bool inZone = false;

    public FurniturePiece From { get => from; }
    public FurniturePiece To { get => to; }
    public Transform Anchor { get => anchor; }

    public void InsertLinker(Linker l)
    {
        linker = l;
        l.transform.rotation = anchor.rotation;
        l.transform.position = anchor.position;
        l.transform.position += anchor.position - l.transform.position;
        gameObject.SetActive(false);
        l.transform.parent = from.transform;
        SFX_Controller.Instance.CreateSFX(SFX_Type.PopInWood, l.transform.position, l.transform.forward);
    }

    public void RemoveLinker()
    {
        linker = null;
    }

    public void LinkLinks(Link other)
    {
        SFX_Controller.Instance.CreateSFX(SFX_Type.PopIn, anchor.position, anchor.forward);

        other.to = from;
        to = other.from;

        if (other.linker != null)
            other.linker.ToLink = this;
        else
            linker.ToLink = other;

        other.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Place(Link f)
    {
        f.from.transform.parent = from.transform;
        f.from.transform.localRotation = Quaternion.identity;

        f.from.transform.position = anchor.position;
        f.from.transform.position += anchor.position - f.anchor.position;

        Transform t = new GameObject("rot").transform;

        t.position = anchor.position;
        t.rotation = Quaternion.LookRotation(-anchor.forward, -anchor.up);

        f.from.transform.parent = t;
        t.rotation = f.anchor.rotation;

        f.gameObject.SetActive(false);
        gameObject.SetActive(false);

        f.from.transform.parent = from.transform;
        Destroy(t.gameObject);
    }

    public void StartGrab()
    {
        grabed = true;
        if (linker)
            linker.StartGrab();
    }

    public void StopGrab()
    {
        grabed = false;
        if (linker)
            linker.StopGrab();
    }

    private void DetacheLink()
    {
        inZone = false;
        to = null;

        if (linker == null)
        {
            gameObject.SetActive(true);
        }
        else
        {
            linker.ResetLink();
        }
    }

    public void Detache()
    {
        if (to != null)
        {
            from.AddForceNextPhysFrame(anchor.position, -anchor.forward * 200f);

            Linker linkerToUse = null;
            Link otherLink = null;

            if (linker)
            {
                //I got the linker of this link
                linkerToUse = linker;
                otherLink = linkerToUse.ToLink;
            }
            else
            {
                //The linker is on the other side
                linkerToUse = to.FindSetByToLink(this).linker;
                otherLink = linkerToUse.Origine;
            }

            if (neededLinks != null && neededLinks.Length != 0)
            {
                foreach (Link lnk in neededLinks)
                {
                    if (lnk.to != null)
                    {
                        FurniturePiece.LinkSet set = null;
                        if (lnk.linker == null)
                        {
                            set = lnk.to.FindSetByToLink(lnk);
                        }
                        else
                        {
                            set = new FurniturePiece.LinkSet(lnk, lnk.linker.ToLink, lnk.linker);
                        }
                        set.from.DetacheLink();
                        set.to.DetacheLink();

                        SFX_Controller.Instance.CreateSFX(SFX_Type.PopOut, set.from.anchor.position, set.from.anchor.forward);

                        set.linker.Origine.from.RemoveSet(set.linker);
                        set.linker.ToLink.from.RemoveSet(set.linker);
                    }
                }
            }
            else if (otherLink.neededLinks != null && otherLink.neededLinks.Length != 0)
            {
                
                FurniturePiece.LinkSet[] sets = otherLink.from.FindAllOtherLinkAttachedToPiece(from);
                foreach (Link lnk in otherLink.neededLinks)
                {
                    foreach (FurniturePiece.LinkSet set in sets)
                    {
                        Debug.LogError(lnk.from.name + "  " + lnk.to.name + "   " + lnk.linker.name);
                        Debug.LogError(set.Print() + " -  -- --- --  --");

                        if (set.from == lnk)
                        {
                            set.from.DetacheLink();
                            set.to.DetacheLink();

                            SFX_Controller.Instance.CreateSFX(SFX_Type.PopOut, set.from.anchor.position, set.from.anchor.forward);

                            set.linker.Origine.from.RemoveSet(set.linker);
                            set.linker.ToLink.from.RemoveSet(set.linker);
                        }
                    }
                }
            }

            SFX_Controller.Instance.CreateSFX(SFX_Type.PopOut, anchor.position, anchor.forward);

            linkerToUse.Origine.from.RemoveSet(linkerToUse);
            linkerToUse.ToLink.from.RemoveSet(linkerToUse);

            linkerToUse.Origine.DetacheLink();
            linkerToUse.ToLink.DetacheLink();
        }
    }

    public static bool AuthorizeLink(Link selfLink, Link otherLink)
    {
        bool isOk = true;

        float dotX = Vector3.Dot(selfLink.anchor.up, otherLink.anchor.up);
        float dotZ = Vector3.Dot(selfLink.anchor.forward, otherLink.anchor.forward);

        isOk = dotZ < -0.75f;

        if (selfLink.neededLinks != null)
        {
            if (selfLink.neededLinks.Length != 0)
            {
                isOk = dotX < -0.75f;

                foreach (Link h in selfLink.neededLinks)
                {
                    if (h.inZone == false)
                    {
                        isOk = false;
                        break;
                    }
                }
            }
        }

        Debug.LogError(dotX + "   " + (dotZ < -0.75f) + "   " + isOk);

        return isOk;
    }

    public void PlacePiece(Linker l, Link other)
    {
        from.AddSet(this, other, l);
        other.from.AddSet(other, this, l);
        PlayerController.Instance.ReleaseCommand();
        from.PlaceComponent(other, this);
        LinkLinks(other);

        if(neededLinks != null && neededLinks.Length != 0)
        {
            for (int i = 0; i < neededLinks.Length; i++)
            {
                if (neededLinks[i].linker == null)
                {
                    neededLinks[i].LinkLinks(neededLinks[i].linkInZone.Origine);

                    neededLinks[i].from.AddSet(neededLinks[i], neededLinks[i].linkInZone.Origine, neededLinks[i].linkInZone);
                    neededLinks[i].linkInZone.Origine.from.AddSet(neededLinks[i].linkInZone.Origine, neededLinks[i], neededLinks[i].linkInZone);
                    neededLinks[i].linkInZone.IsUsed = true;
                }
                else
                {
                    neededLinks[i].LinkLinks(neededLinks[i].linker.ToLink);

                    neededLinks[i].from.AddSet(neededLinks[i], neededLinks[i].linker.ToLink, neededLinks[i].linker);
                    neededLinks[i].linker.ToLink.from.AddSet(neededLinks[i].linker.ToLink, neededLinks[i], neededLinks[i].linker);
                    neededLinks[i].linker.IsUsed = true;
                }
            }
        }

        l.IsUsed = true;
    }

    public void OnTriggerExit(Collider other)
    {
        Linker l = other.GetComponent<Linker>();
        if (l)
        {
            linkInZone = l;
            inZone = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Linker l = other.GetComponent<Linker>();
        if (l)
        {
            linkInZone = l;
            inZone = true;
        }

        if (l != null)
        {
            if (!l.IsUsed)
            {
                if (grabed)
                {
                    if (l.IsLinked)
                    {
                        if (!l.IsGrabed)
                        {
                            if (AuthorizeLink(this, l.Origine))
                            {
                                this.PlacePiece(l, l.Origine);
                            }
                        }
                    }
                }
                else
                {
                    if (!l.IsLinked)
                    {
                        if (l.IsGrabed)
                        {
                            l.PlaceLinker(this);
                        }
                    }
                    else
                    {
                        if (l.IsGrabed)
                        {
                            if (AuthorizeLink(l.Origine, this))
                            {
                                l.Origine.PlacePiece(l, this);
                            }
                        }
                    }
                }
            }
        }


        Tool t = other.GetComponent<Tool>();
        if (t)
        {
            if (t.toolType == ToolType.Glue && t.IsGrabed)
            {
                if(!glueIn)
                {
                    meshRenderer.material.color = Color.yellow;
                    glueIn = true;
                    SFX_Controller.Instance.CreateSFX(SFX_Type.Glue, anchor.position, anchor.forward);
                }
            }
        }
    }
}
