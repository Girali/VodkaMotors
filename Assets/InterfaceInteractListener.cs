using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceInteractListener : MonoBehaviour
{
    [SerializeField]
    Color idle = Color.white;
    [SerializeField]
    Color active = Color.yellow;

    [SerializeField]
    UnityEngine.UI.Graphic[] imgs;

    public void RecieveNotify(bool b)
    {
        for (int i = 0; i < imgs.Length; i++)
        {
            imgs[i].color = b ? active : idle;
        }
    }
}
