using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_PartsPanel : MonoBehaviour
{
    public BasePartController basePartController;
    public GameObject parent;
    public GameObject part;

    public GameObject prev;
    public GameObject next;

    private List<UI_PartSeparator> partSeparators = new List<UI_PartSeparator>();
    private UI_PartSeparator currentSection;


    public VehiculPartObject OnClick(VehiculPartObject vpo)
    {
        return basePartController.ReplacePiece(vpo);
    }

    public void EmptySection()
    {
        int i = 0;
        GameObject[] allChildren = new GameObject[parent.transform.childCount];

        foreach (Transform child in parent.transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        foreach (GameObject child in allChildren)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowVariant(bool b)
    {
        prev.SetActive(b);
        next.SetActive(b);
    }

    public void Next()
    {
        basePartController.NextIndex();

        foreach (UI_PartSeparator ps in partSeparators)
        {
            if (ps.opened)
                ps.EmptySection();
        }

        currentSection.FillSection();
    }

    public void Prev()
    {
        basePartController.PrevIndex();

        foreach (UI_PartSeparator ps in partSeparators)
        {
            if (ps.opened)
                ps.EmptySection();
        }

        currentSection.FillSection();
    }

    public bool IsEmpty(VehiculParts vp)
    {
        switch (vp)
        {
            case VehiculParts.Engine:
                return false;
            case VehiculParts.Wheel:
                return false;
            case VehiculParts.Seat:
                return false;
            case VehiculParts.Roof:
                return true;
            case VehiculParts.Armature:
                return true;
            case VehiculParts.StairingWheel:
                return false;
            case VehiculParts.Back:
                return true;
            case VehiculParts.Carrige:
                return false;
            case VehiculParts.Spoiler:
                return true;
            case VehiculParts.Sidebar:
                return true;
            case VehiculParts.Bumper:
                return true;
            case VehiculParts.Exaust:
                return true;
            case VehiculParts.Bonnet:
                return true;
            case VehiculParts.Door:
                return true;
            default:
                break;
        }
        return false;
    }

    public void AddEmptyPart(VehiculParts vpo)
    {
        Predicate<UI_PartSeparator> pred = (p) => p.part == vpo;

        if (!partSeparators.Exists(pred) && IsEmpty(vpo))
        {
            GameObject g = Instantiate(part, parent.transform);
            g.SetActive(true);
            UI_PartSeparator ui = g.GetComponent<UI_PartSeparator>();
            ui.Init(vpo, this, IsEmpty(vpo));
            partSeparators.Add(ui);
        }
    }

    public void AddPart(VehiculPartObject vpo)
    {
        Predicate<UI_PartSeparator> pred = (p) => p.part == vpo.part;

        if (partSeparators.Exists(pred))
        {
            int i = partSeparators.FindIndex(pred);
            partSeparators[i].AddVPO(vpo);
        }
        else
        {
            GameObject g = Instantiate(part, parent.transform);
            g.SetActive(true);
            UI_PartSeparator ui = g.GetComponent<UI_PartSeparator>();
            ui.Init(vpo.part, this, IsEmpty(vpo.part));
            partSeparators.Add(ui);
            ui.AddVPO(vpo);
        }
    }

    public void OpenPart(UI_PartSeparator s)
    {
        basePartController.SetTarget(s.part);

        foreach (UI_PartSeparator ps in partSeparators)
        {
            if (ps.opened)
                ps.EmptySection();
        }

        s.FillSection();
        currentSection = s;
    }

    public void OnEnable()
    {
        EmptySection();

        foreach (VehiculPartObject vpo in basePartController.partStored)
        {
            AddPart(vpo);
        }

        for (VehiculParts i = VehiculParts.Engine; i < VehiculParts.Count; i++)
        {
            int itr = 0;

            if (basePartController.vehiculPartController.GetPart(i, itr) != null)
            {
                AddEmptyPart(i);
            }

            if (i == VehiculParts.Wheel)
            {
                for (; itr < 4; itr++)
                {
                    if(basePartController.vehiculPartController.GetPart(i, itr) != null)
                    {
                        AddEmptyPart(i);
                    }
                }
            }

            if (i == VehiculParts.Door)
            {
                for (; itr < 2; itr++)
                {
                    if (basePartController.vehiculPartController.GetPart(i, itr) != null)
                    {
                        AddEmptyPart(i);
                    }
                }
            }
        }

        partSeparators[0].Open();
    }
}
