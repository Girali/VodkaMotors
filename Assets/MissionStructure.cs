using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStructure : WorldStructre
{
    private bool missionPoint = false;
    public AudioSource source;
    public ParticleSystem system;

    public override void Init()
    {
        base.Init();
        GameController.Instance.AddMissionPoint(this);
    }

    public void StartMission()
    {
        missionPoint = true;
        DUI_Controller.Instance.AddQuestPoint(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (missionPoint)
        {
            CrateContentManager ccm = other.GetComponent<CrateContentManager>();
            if (ccm != null)
            {
                missionPoint = false;
                ccm.Complete();
            }
        }
    }
}
