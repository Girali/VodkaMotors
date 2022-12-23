using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PartCell : MonoBehaviour
{
    private VehiculPartObject vehiculPartObject;
    private UI_PartSeparator partSeparator;
    public Image icon;

    public void Init(VehiculPartObject vpo, UI_PartSeparator ui)
    {
        partSeparator = ui;
        vehiculPartObject = vpo;
        icon.sprite = vpo.Sprite;
    }

    public void Click()
    {
        partSeparator.OnClick(vehiculPartObject);
    }
}
