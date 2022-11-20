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
    private GameObject player;
    public Transform playerSpawnPoint;

    public void WorldGenFinished()
    {
        player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
    }
}
