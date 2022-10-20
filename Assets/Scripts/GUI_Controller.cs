using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent notifyLeftClickDown;
    public UnityEvent notifyMidleClickDown;
    public UnityEvent notifyRightClickDown;

    public UnityEvent notifyLeftClickUp;
    public UnityEvent notifyMidleClickUp;
    public UnityEvent notifyRightClickUp;

    public UnityEvent notifyScrollDown;
    public UnityEvent notifyScrollUp;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            notifyLeftClickDown.Invoke();

        if (Input.GetMouseButtonDown(2))
            notifyMidleClickDown.Invoke();

        if (Input.GetMouseButtonDown(1))
            notifyRightClickDown.Invoke();

        if (Input.GetMouseButtonUp(0))
            notifyLeftClickUp.Invoke();

        if (Input.GetMouseButtonUp(2))
            notifyMidleClickUp.Invoke();

        if (Input.GetMouseButtonUp(1))
            notifyRightClickUp.Invoke();

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            notifyScrollDown.Invoke();

        if(Input.GetAxis("Mouse ScrollWheel") == 0)
            notifyScrollUp.Invoke();

    }

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
