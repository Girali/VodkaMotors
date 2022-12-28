using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ControlPanel : MonoBehaviour
{
    public TMP_Text forawrd;
    public TMP_Text left;

    private void OnEnable()
    {
        forawrd.text = "Forward : " + AppController.Instance.forward;
        left.text = "Left : " + AppController.Instance.left;
    }
}
