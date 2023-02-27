using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MasterVolumeSet : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Slider>().SetValueWithoutNotify(AppController.Instance.Volume);
    }

    public void ChangeVolume(float f)
    {
        AppController.Instance.Volume = f;
        AudioListener.volume = f;
    }
}
