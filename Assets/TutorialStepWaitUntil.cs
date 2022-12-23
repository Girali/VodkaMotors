using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStepWaitUntil : TutorialStep
{
    public float timer = 0;

    public override void Begin()
    {
        base.Begin();
        StartCoroutine(CRT_Wait());
        onStart.Invoke();
    }

    IEnumerator CRT_Wait()
    {
        yield return new WaitForSeconds(timer);
        End();
    }

    public override void End()
    {
        base.End();
        onEnd.Invoke();
    }
}
