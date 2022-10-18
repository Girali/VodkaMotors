using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Controller : MonoBehaviour
{
    private static GUI_Controller _instance;
    public static GUI_Controller Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GUI_Controller>();
            }

            return _instance;
        }
    }

    public GameObject[] toolIcons;

    public void SetTool(ToolType t)
    {
        toolIcons[0].SetActive(false);
        toolIcons[1].SetActive(false);
        toolIcons[2].SetActive(false);
        toolIcons[3].SetActive(false);

        switch (t)
        {
            case ToolType.Hand:
                toolIcons[0].SetActive(true);
                break;
            case ToolType.Glue:
                toolIcons[1].SetActive(true);
                break;
            case ToolType.Screwdriver:
                toolIcons[2].SetActive(true);
                break;
            case ToolType.Hexkey:
                toolIcons[3].SetActive(true);
                break;
            default:
                break;
        }
    }
}
