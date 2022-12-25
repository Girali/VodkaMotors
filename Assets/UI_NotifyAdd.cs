using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_NotifyAdd : MonoBehaviour
{
    public TMP_Text text;
    public Image sprite;


    public void Init(string y)
    {
        gameObject.SetActive(true);
        text.text = y;
    }

    public void Init(VehiculPartObject vpo)
    {
        gameObject.SetActive(true);
        text.text = vpo.name;
        sprite.sprite = vpo.Sprite;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
