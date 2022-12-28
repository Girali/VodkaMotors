using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Ammo : MonoBehaviour
{
    public TMP_Text currentText;
    public TMP_Text totalText;

    public void UpdateView(int current, int total)
    {
        currentText.text = current.ToString();
        totalText.text = total.ToString();
    }
}
