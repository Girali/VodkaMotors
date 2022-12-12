using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameController>();
            }

            return instance;
        }
    }

    public GameObject playerPrefab;
    public GameObject player;
    public GameObject[] prefabsMission;

    public Transform playerSpawnPoint;

    private List<MissionStructure> missionPoints = new List<MissionStructure>();
    private MissionStructure startPoint = null;
    private MissionStructure endPoint = null;

    public void CompleteMission()
    {

    }

    public void StartNewMission()
    {

    }

    public void SpawnMissionObject(Transform t)
    {
        int i = Random.Range(0, prefabsMission.Length);
        Instantiate(prefabsMission[i], t.position + new Vector3(0,5,0), Quaternion.identity);
    }

    public void StartFirstMission()
    {
        List<MissionStructure> m = new List<MissionStructure>(missionPoints);
        m.Sort(new MissionDistCompare());
        endPoint = m[0];
        endPoint.StartMission();
        SpawnMissionObject(player.transform);

        GUI_Controller.Instance.compass.InitMission(player, endPoint);
    }

    class MissionDistCompare : IComparer<MissionStructure>
    {
        public int Compare(MissionStructure e1, MissionStructure e2)
        {
            return e1.DistanceFromPlayer().CompareTo(e2.DistanceFromPlayer());
        }
    }

    public void FinishMission()
    {

    }

    public void AddMissionPoint(MissionStructure missionStructure)
    {
        missionPoints.Add(missionStructure);
    }

    public void WorldGenFinished()
    {
        player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
        StartFirstMission();
    }
}
