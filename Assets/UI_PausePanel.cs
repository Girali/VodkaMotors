using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PausePanel : MonoBehaviour
{
    public void Resume()
    {
        AppController.Instance.Unpause();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
