using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyAnimCallback : MonoBehaviour
{
    public EnnemySoundController ennemySoundController;

    public void FootStep()
    {
        ennemySoundController.FootStep();
    }
}
