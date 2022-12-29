using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiculPartHolder : MonoBehaviour
{
    private static VehiculPartHolder instance;
    public static VehiculPartHolder Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<VehiculPartHolder>();
            }

            return instance;
        }
    }

    public GameObject[] parts;

    public GameObject GetRandomPart()
    {
        return parts[Random.Range(0, parts.Length)];
    }
}
