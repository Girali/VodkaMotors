using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStructure : WorldStructre
{
    private bool missionPoint = false;

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

    public float DistanceFromPlayer()
    {
        return Vector3.Distance(transform.position, GameController.Instance.player.transform.position);
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
