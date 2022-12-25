using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public GameObject gui;

    public UI_Compass compass;
    public UI_Vodka vodka;
    public UI_PartsPanel partsPanel;

    [SerializeField]
    private TMP_Text interactText;

    public GameObject addNotify;
    public GameObject addDialogue;

    public void SetInteractText(string s)
    {
        interactText.text = s;
    }

    public void AddNotify(VehiculPartObject vpo)
    {
        GameObject g = Instantiate(addNotify, gui.transform);
        g.GetComponent<UI_NotifyAdd>().Init(vpo);
    }

    public void AddDialogue(DialogueController.Sequence s)
    {
        GameObject g = Instantiate(addDialogue, gui.transform);
        g.GetComponent<UI_DiaolgueNotify>().InitText(s.text);
    }


    public Jun_TweenRuntime fadeIn;
    public Jun_TweenRuntime fadeOut;
    public Jun_TweenRuntime fadeOutLong;

    //public GameObject[] toolIcons;
    //public GameObject helpPanel;
    //public GameObject[] helpPanels;
    //
    //public UnityEvent notifyLeftClickDown;
    //public UnityEvent notifyMidleClickDown;
    //public UnityEvent notifyRightClickDown;
    //
    //public UnityEvent notifyLeftClickUp;
    //public UnityEvent notifyMidleClickUp;
    //public UnityEvent notifyRightClickUp;
    //
    //public UnityEvent notifyScrollDown;
    //public UnityEvent notifyScrollUp;
    //
    //public enum HelpStep
    //{
    //    None,
    //    Hand,
    //    Grab,
    //    View
    //}
    //
    //HelpStep currentHelpStep = HelpStep.None;
    //
    //public void SetStep(HelpStep s)
    //{
    //    if(s != currentHelpStep)
    //    {
    //        currentHelpStep = s;
    //        for (int i = 0; i < helpPanels.Length; i++)
    //        {
    //            helpPanels[i].SetActive(i + 1 == (int)s);
    //        }
    //    }
    //}
    //
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //        helpPanel.SetActive(!helpPanel.activeSelf);
    //
    //    if (Input.GetMouseButtonDown(0))
    //        notifyLeftClickDown.Invoke();
    //
    //    if (Input.GetMouseButtonDown(2))
    //        notifyMidleClickDown.Invoke();
    //
    //    if (Input.GetMouseButtonDown(1))
    //        notifyRightClickDown.Invoke();
    //
    //    if (Input.GetMouseButtonUp(0))
    //        notifyLeftClickUp.Invoke();
    //
    //    if (Input.GetMouseButtonUp(2))
    //        notifyMidleClickUp.Invoke();
    //
    //    if (Input.GetMouseButtonUp(1))
    //        notifyRightClickUp.Invoke();
    //
    //    if (Input.GetAxis("Mouse ScrollWheel") != 0)
    //        notifyScrollDown.Invoke();
    //
    //    if(Input.GetAxis("Mouse ScrollWheel") == 0)
    //        notifyScrollUp.Invoke();
    //
    //}
    //
    //public void SetTool(ToolType t)
    //{
    //    toolIcons[0].SetActive(false);
    //    toolIcons[1].SetActive(false);
    //    toolIcons[2].SetActive(false);
    //    toolIcons[3].SetActive(false);
    //
    //    switch (t)
    //    {
    //        case ToolType.Hand:
    //            toolIcons[0].SetActive(true);
    //            break;
    //        case ToolType.Glue:
    //            toolIcons[1].SetActive(true);
    //            break;
    //        case ToolType.Screwdriver:
    //            toolIcons[2].SetActive(true);
    //            break;
    //        case ToolType.Hexkey:
    //            toolIcons[3].SetActive(true);
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
