using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DUI_Health : MonoBehaviour
{
    public Image fill;
    public GameObject gui;

    public void Fill(float f)
    {
        if (f <= 0)
            gui.SetActive(false);
        else if(f != 1)
            gui.SetActive(true);


        fill.fillAmount = f;
    }
}
