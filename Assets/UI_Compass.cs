using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Compass : MonoBehaviour
{
    [SerializeField]
    private GameObject item;
    [SerializeField]
    private GameObject mission;

    private GameObject player;
    private MissionStructure missionStructure;
    private CrateContentManager crateContentManager;

    private float threshold = 0.5f;
    [SerializeField]
    private Transform positionLeft;
    [SerializeField]
    private Transform positionRight;

    public void InitMission(CrateContentManager ccm)
    {
        crateContentManager = ccm;
    }

    public void InitMission(GameObject p, MissionStructure ms)
    {
        missionStructure = ms;
        player = p;
    }

    public void Update()
    {
        if (crateContentManager != null && player != null)
        {
            Vector3 dir = (crateContentManager.transform.position - player.transform.position);
            dir.y = 0;
            dir = dir.normalized;

            float f = Vector3.Dot(player.transform.forward, dir) * (1 / threshold) - 1f;
            float r = Vector3.Dot(player.transform.right, dir);

            float t = 0;

            if (r > 0)
            {
                t = (1 - (f / 2f));
            }
            else
            {
                t = (f / 2f);
            }

            if (f < -2)
            {
                item.transform.position = Vector3.Lerp(positionLeft.position, positionRight.position, t);
            }
            else
            {
                item.transform.position = Vector3.Lerp(item.transform.position, Vector3.Lerp(positionLeft.position, positionRight.position, t), 0.1f);
            }
        }


        if (missionStructure != null && player != null) 
        {
            Vector3 dir = (missionStructure.transform.position - player.transform.position);
            dir.y = 0;
            dir = dir.normalized;

            float f = Vector3.Dot(player.transform.forward, dir) * (1 / threshold) - 1f;
            float r = Vector3.Dot(player.transform.right, dir);

            float t = 0;

            if(r > 0)
            {
                t = (1 - (f / 2f));
            }
            else
            {
                t = (f / 2f);
            }

            if (f < -2)
            {
                mission.transform.position = Vector3.Lerp(positionLeft.position, positionRight.position, t);
            }
            else
            {
                mission.transform.position = Vector3.Lerp(mission.transform.position, Vector3.Lerp(positionLeft.position, positionRight.position, t), 0.1f);
            }
        }
    }
}
