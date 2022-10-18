using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureController : MonoBehaviour
{
    public GameObject[] bases;
    public GameObject[] legs;
    public GameObject[] backs;
    public GameObject[] handrests;
    public GameObject link;
    private int linkCount = 0;

    private void Start()
    {
        GenerateFurniture();
    }

    public void GenerateFurniture()
    {
        linkCount = 0;
        //int indexBase = Random.Range(0, bases.Length);
        //BaseFurniturePiece b = bases[indexBase].GetComponent<BaseFurniturePiece>();
        List<FurnitureSet> l = new List<FurnitureSet>();
        l.Add(new FurnitureSet(FurnitureType.Base, AnchorType.None));
        ProcessSets(l);

        for (int i = 0; i < linkCount; i++)
        {
            Instantiate(link);
        }
    }

    private void ProcessSets(List<FurnitureSet> sets)
    {
        foreach (FurnitureSet item in sets)
        {
            FurniturePiece fp = SpawnPieces(GetRandomPiece(item));
            if (fp)
            {
                ProcessSets(fp.Init());
            }
        }
    }

    private FurniturePiece SpawnPieces(FurniturePiece s)
    {
        switch (s.type)
        {
            case FurnitureType.Base:
                return Instantiate(s.gameObject).GetComponent<FurniturePiece>();
            case FurnitureType.Leg:
                linkCount += 4;
                switch (((LegFurniturePiece)s).currentType)
                {
                    case LegFurniturePiece.Type.Double:
                        Instantiate(s.gameObject).GetComponent<FurniturePiece>().Init();
                        Instantiate(s.gameObject).GetComponent<FurniturePiece>().Init();
                        break;
                    case LegFurniturePiece.Type.Single:
                        Instantiate(s.gameObject).GetComponent<FurniturePiece>().Init();
                        Instantiate(s.gameObject).GetComponent<FurniturePiece>().Init();
                        Instantiate(s.gameObject).GetComponent<FurniturePiece>().Init();
                        Instantiate(s.gameObject).GetComponent<FurniturePiece>().Init();
                        break;
                }
                break;
            case FurnitureType.Back:
                linkCount += 1;
                return Instantiate(s.gameObject).GetComponent<FurniturePiece>();
            case FurnitureType.Handrest:
                linkCount += 2;
                Instantiate(s.gameObject).GetComponent<FurniturePiece>().Init();
                HandrestFurniturePiece fp = Instantiate(s.gameObject).GetComponent<HandrestFurniturePiece>();
                fp.currentType = HandrestFurniturePiece.Type.Left;
                fp.Init();
                break;
            default:
                break;
        }

        return null;
    }

    private FurniturePiece GetRandomPiece(FurnitureSet s)
    {
        FurniturePiece fp = null;

        do
        {
            switch (s.type)
            {
                case FurnitureType.Base:
                    fp = bases[Random.Range(0, bases.Length)].GetComponent<FurniturePiece>();
                    break;
                case FurnitureType.Leg:
                    fp = legs[Random.Range(0, legs.Length)].GetComponent<FurniturePiece>();
                    break;
                case FurnitureType.Back:
                    fp = backs[Random.Range(0, backs.Length)].GetComponent<FurniturePiece>();
                    break;
                case FurnitureType.Handrest:
                    fp = handrests[Random.Range(0, handrests.Length)].GetComponent<FurniturePiece>();
                    break;
                default:
                    break;
            }
        }
        while (s.type != fp.type && s.anchorType != fp.anchorType);

        return fp;
    }
}


public class Furniture
{
    public BaseFurniturePiece basePiece;
    public LegFurniturePiece[] legPieces;
    public BackFurniturePiece backPiece;
    public HandrestFurniturePiece[] handrestPiece;
}