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

    public bool glueIn = false;
    public Linker linker = null;
    private bool grabed = false;

    public FurniturePiece From { get => from; }

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

            return true;
        }
        else
        {
            linker = null;
            return false;
        }
    }

    public void Place(Link f)
    {
        f.from.transform.rotation *= f.anchor.rotation * Quaternion.Inverse(anchor.rotation);

        f.from.transform.position = anchor.position;
        f.from.transform.position += anchor.position - f.anchor.position;

        f.gameObject.SetActive(false);
        gameObject.SetActive(false);

        f.to = from;
        to = f.from;

        f.from.transform.parent = from.transform;
    }

    public void StartGrab()
    {
        grabed = true;
        if(linker)
        linker.StartGrab();
    }

    public void StopGrab()
    {
        grabed = false;
        if(linker)
        linker.StopGrab();
    }

    private void OnTriggerEnter(Collider other)
    {
        Linker l = other.GetComponent<Linker>();

        if (grabed)
        {
            if (l)
            {
                if (l.IsLinked)
                {
                    if (!l.IsGrabed)
                    {
                        l.PlacePiece(from, this);
                    }
                }
            }
        }
        else
        {
            if (l)
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
                        l.PlacePiece(from, this);
                    }
                }
            }
        }
    }
}
