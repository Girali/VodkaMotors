using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DiaolgueNotify : MonoBehaviour
{
    public TMP_Text areaText;

    public Jun_TweenRuntime fadeIn;
    public Jun_TweenRuntime fadeOut;

    public void InitText(string s)
    {
        gameObject.SetActive(true);
        StartCoroutine(CRT_TextShow(s));
    }

    IEnumerator CRT_TextShow(string s)
    {
        fadeIn.Play();

        yield return new WaitForSeconds(.5f);

        string[] ss = s.Split(' ');

        string text = "";
        for (int i = 0; i < ss.Length; i++)
        {
            text += ss[i] + " ";
            areaText.text = text;
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(4f);

        fadeOut.Play();
    }
}
