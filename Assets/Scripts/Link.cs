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

    public Link[] neededLinks;
    public bool glueIn = false;
    public Linker linker = null;
    private bool grabed = false;

    public FurniturePiece From { get => from; }
    public FurniturePiece To { get => to; }
    public Transform Anchor { get => anchor; }

    public bool InsertLinker(Linker l)
    {
        if (linker == null)
        {
            linker = l;

            l.transform.rotation = anchor.rotation;

            l.transform.position = anchor.position;
            l.transform.position += anchor.position - l.transform.position;

            gameObject.SetActive(false);

            l.transform.parent = from.transform;

            SFX_Controller.Instance.CreateSFX(SFX_Type.PopInWood, l.transform.position, l.transform.forward);

            return true;
        }
        else
        {
            linker = null;
            return false;
        }
    }

    public void PlaceInverse(Link f)
    {
        from.transform.parent = f.from.transform;
        from.transform.localRotation = Quaternion.identity;

        from.transform.position = f.anchor.position;
        from.transform.position += f.anchor.position - anchor.position;

        Transform t = new GameObject("rot").transform;

        t.position = f.anchor.position;
        t.rotation = Quaternion.LookRotation(-anchor.forward, -anchor.up);

        from.transform.parent = t;
        t.rotation = f.anchor.rotation;

        f.gameObject.SetActive(false);
        gameObject.SetActive(false);

        SFX_Controller.Instance.CreateSFX(SFX_Type.PopIn, anchor.position, anchor.forward);

        f.to = from;
        to = f.from;

        from.transform.parent = f.from.transform;
        Destroy(t.gameObject);
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

        SFX_Controller.Instance.CreateSFX(SFX_Type.PopIn, f.anchor.position, f.anchor.forward);

        f.to = from;
        to = f.from;

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

    public void Detache(FurniturePiece f)
    {
        if (to != null)
        {
            Link l = to.GetOtherLink(f);
            to = null;
            if (l != null)
                l.Detache(f);
        }

        from.AddForceNextPhysFrame(anchor.position, -anchor.forward * 200f);

        if (linker == null)
        {
            gameObject.SetActive(true);
            SFX_Controller.Instance.CreateSFX(SFX_Type.PopOut, anchor.position, anchor.forward);
        }
        else
        {
            linker.ResetLink();
        }
    }

    public bool inZone = false;

    public void OnTriggerExit(Collider other)
    {
        Linker l = other.GetComponent<Linker>();
        if (l)
            inZone = false;
    }

    public static bool CheckMultiLinkNeed(Link selfLink, Link otherLink)
    {
        if (selfLink.neededLinks != null && otherLink.neededLinks != null)
        {
            if (selfLink.neededLinks.Length == otherLink.neededLinks.Length)
            {
                bool isOk = true;

                foreach (Link h in selfLink.neededLinks)
                {
                    if (h.inZone == false)
                    {
                        isOk = false;
                        break;
                    }
                }

                return isOk;
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Linker l = other.GetComponent<Linker>();
        if(l)
            inZone = true;

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
                            if(CheckMultiLinkNeed(this, l.Link))
                            {

                            }
                            else
                            {

                                l.PlacePiece(from, this);
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
                            if (CheckMultiLinkNeed(this, l.Link))
                            {

                            }
                            else
                            {
                                l.PlacePieceInverse(from, this);
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
