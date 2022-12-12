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
    public GameObject vehicul;
    public GameObject[] prefabsMission;

    public SpawnPoints spawnPoints;

    private List<MissionStructure> missionPoints = new List<MissionStructure>();
    private MissionStructure startPoint = null;
    private MissionStructure endPoint = null;


    private float spawnRadius = 100f;

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

    public (bool, Vector3) TestSpawnPosition(Vector3 v, Vector3 d)
    {
        Vector3 n = Vector3.Cross(Vector3.up, d).normalized * 5.5f;
        Vector3 spawnPointFrwd = v + d;
        Vector3 spawnPointBkwd = v - d;
        Vector3 spawnPointFrwdLeft = v + d + n;
        Vector3 spawnPointBkwdLeft = v - d + n;
        Vector3 spawnPointFrwdRight = v + d - n;
        Vector3 spawnPointBkwdRight = v - d - n;

        RaycastHit hitUnder;
        RaycastHit hitFrwdLeft;
        RaycastHit hitBkwdLeft;
        RaycastHit hitFrwdRight;
        RaycastHit hitBkwdRight; 
        RaycastHit hitFrwd;
        RaycastHit hitBkwd;

        bool bu = Physics.Raycast(v, Vector3.down, out hitUnder, 30);
        bool bfl = Physics.Raycast(spawnPointFrwdLeft, Vector3.down, out hitFrwdLeft, 30);
        bool bbl = Physics.Raycast(spawnPointBkwdLeft, Vector3.down, out hitBkwdLeft, 30);
        bool bfr = Physics.Raycast(spawnPointFrwdRight, Vector3.down, out hitFrwdRight, 30);
        bool bbr = Physics.Raycast(spawnPointBkwdRight, Vector3.down, out hitBkwdRight, 30);
        bool bf = Physics.Raycast(spawnPointFrwd, Vector3.down, out hitFrwd, 30);
        bool bb = Physics.Raycast(spawnPointBkwd, Vector3.down, out hitBkwd, 30);

        if (bu && bfl && bbl && bfr && bbr&& bf && bb)
        {
            float h1 = Mathf.Abs(hitUnder.point.y - hitFrwdLeft.point.y);
            float h2 = Mathf.Abs(hitUnder.point.y - hitBkwdLeft.point.y);
            float h3 = Mathf.Abs(hitUnder.point.y - hitFrwdRight.point.y);
            float h4 = Mathf.Abs(hitUnder.point.y - hitBkwdRight.point.y);
            float h5 = Mathf.Abs(hitUnder.point.y - hitFrwd.point.y);
            float h6 = Mathf.Abs(hitUnder.point.y - hitBkwd.point.y);

            if (h1 < 1 && h2 < 1 && h3 < 1 && h4 < 1 && h5 < 1 && h6 < 1)
            {
                return (true, hitUnder.point);
            }
        }

        return (false, Vector3.zero);
    }

    private IEnumerator SpawnPointFinder()
    {
        int i = 0;

        (bool, Vector3) isCorrectSpawnPosition = (false, Vector3.zero);
        Vector2 v = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPoint = new Vector3(v.x, 20, v.y);
        Vector2 k = Random.insideUnitCircle.normalized * 11f;
        Vector3 direction = new Vector3(k.x, 0, k.y);

        while (!isCorrectSpawnPosition.Item1)
        {
            isCorrectSpawnPosition = TestSpawnPosition(spawnPoint, direction);
            v = Random.insideUnitCircle * spawnRadius;
            spawnPoint = new Vector3(v.x, 20, v.y);
            k = Random.insideUnitCircle.normalized * 11f;
            direction = new Vector3(k.x, 0, k.y);
            i++;
            if(i % 10 == 0)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        spawnPoints.transform.position = isCorrectSpawnPosition.Item2;
        spawnPoints.transform.rotation = Quaternion.LookRotation(direction);

        player.transform.position = spawnPoints.playerSpawnPoint.position;
        player.transform.rotation = spawnPoints.playerSpawnPoint.rotation;

        vehicul.transform.position = spawnPoints.vehiculSpawnPoint.position;
        vehicul.transform.rotation = spawnPoints.vehiculSpawnPoint.rotation;
    }

    public void WorldGenFinished()
    {
        player = Instantiate(playerPrefab, spawnPoints.playerSpawnPoint.transform.position, spawnPoints.playerSpawnPoint.transform.rotation);

        StartCoroutine(SpawnPointFinder());

        StartFirstMission();
    }
}
