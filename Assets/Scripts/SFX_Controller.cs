using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Controller : MonoBehaviour
{
    private static SFX_Controller _instance;
    public static SFX_Controller Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SFX_Controller>();
            }

            return _instance;
        }
    }

    public GameObject popOut;
    public GameObject popIn;
    public GameObject popInLight;
    public GameObject glue;

    public void CreateSFX(SFX_Type t, Vector3 v, Vector3 f)
    {
        GameObject g;
        switch (t)
        {
            case SFX_Type.PopIn:
                g = Instantiate(popIn, v, Quaternion.LookRotation(f));
                Destroy(g,1f);
                break;
            case SFX_Type.PopInWood:
                g = Instantiate(popInLight, v, Quaternion.LookRotation(f));
                Destroy(g, 1f);
                break;
            case SFX_Type.PopOut:
                g = Instantiate(popOut, v, Quaternion.LookRotation(f));
                Destroy(g, 1f);
                break;
            case SFX_Type.Glue:
                g = Instantiate(glue, v, Quaternion.LookRotation(f));
                Destroy(g, 1f);
                break;
            default:
                break;
        }
    } 
}

public enum SFX_Type
{
    PopIn,
    PopInWood,
    PopOut,
    Glue
}