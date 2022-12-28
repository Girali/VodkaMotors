using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour
{
    public Image fill;

    public void UpdateView(float f)
    {
        fill.fillAmount = f;
    }
}
