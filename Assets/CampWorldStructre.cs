using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampWorldStructre : WorldStructre
{
    public Vector2Int partsSpawnLimit;
    public GameObject[] spawnPoints;
    public float range = 40;

    private List<GameObject> spawnedParts = new List<GameObject>();
    private bool spawned = false;
    private bool inRange = false;

    public void SpawnParts()
    {
        int r = Random.Range(partsSpawnLimit.x, partsSpawnLimit.y);

        for (int i = 0; i < r; i++)
        {
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject part = VehiculPartHolder.Instance.GetRandomPart();
            GameObject g = Instantiate(part, spawnPoint.transform.position, Quaternion.identity);
            spawnedParts.Add(g);
            g.GetComponent<InteractablePart>().StartInteraction();
            g.GetComponent<InteractablePart>().onInteractStart += SubSpawnedObject;
        }
    }

    public void SubSpawnedObject(Interactable io)
    {
        spawnedParts.Remove(io.gameObject);
    }

    private void Update()
    {
        if (GameController.Instance.player)
        {
            float distance = Vector3.Distance(GameController.Instance.player.transform.position, transform.position);

            if (range > distance)
            {
                if (!inRange)
                {
                    inRange = true;

                    if (spawned)
                    {
                        ShowParts(true);
                    }
                    else
                    {
                        spawned = true;
                        SpawnParts();
                    }
                }
            }
            else
            {
                if (inRange)
                {
                    inRange = false;

                    if (spawned)
                    {
                        ShowParts(false);
                    }
                }
            }
        }
    }

    private void ShowParts(bool b)
    {
        foreach (GameObject p in spawnedParts)
        {
            if (p != null)
                p.SetActive(b);
        }
    }
}
