using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStepWaitCallback : TutorialStep
{
    bool started = false;

    public override void Begin()
    {
        if (!started)
        {
            base.Begin();
            onStart.Invoke();
            started = true;
        }
    }

    public override void End()
    {
        if (started)
        {
            base.End();
            onEnd.Invoke();
            started = false;
        }
    }
}
