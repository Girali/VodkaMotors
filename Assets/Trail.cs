using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public float timer;

    public void Init(float distance)
    {
        StartCoroutine(CRT_Life(distance * 0.8f));
    }

    IEnumerator CRT_Life(float d)
    {
        int i = 0;
        float target = (timer * 60f);

        while (target > i)
        {
            float t = i / target;
            transform.localScale = new Vector3(1, 1, Mathf.Lerp(d, 0, t));
            i++;
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
