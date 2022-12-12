using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Vodka : MonoBehaviour
{
    [SerializeField]
    private Image fill;
    [SerializeField]
    private Jun_TweenRuntime drink;

    public void Empty(float f, float et)
    {
        StartCoroutine(CRT_Fill(f, et));
    }
    
    public void UpdateView(float f)
    {
        fill.fillAmount = f;
    }

    private IEnumerator CRT_Fill(float f, float et)
    {
        float t = 0;
        float sv = fill.fillAmount;

        drink.Play();

        while (t > et)
        {
            t += Time.deltaTime;
            fill.fillAmount = Mathf.Lerp(sv, f, t / et);
            yield return new WaitForEndOfFrame();
        }

        fill.fillAmount = f;
    }
}
