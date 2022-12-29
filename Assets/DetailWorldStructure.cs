using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailWorldStructure : WorldStructre
{
    public GameObject[] grasses;
    public Vector2Int grassSpawnLimit;
    public float grassSpawnRange = 40;

    public GameObject[] trees;
    public Vector2Int treeSpawnLimit;
    public float treeSpawnRange = 40;

    public float range = 70;

    private List<GameObject> spawnedParts = new List<GameObject>();
    private bool spawned = false;
    private bool inRange = false;

    private int skipFrame = 5;

    public IEnumerator SpawnParts()
    {
        int s = 0;
        int mask = LayerMask.GetMask("Floor");
        int r = Random.Range(treeSpawnLimit.x, treeSpawnLimit.y);

        for (int i = 0; i < r; i++)
        {
            Vector2 rng = Random.insideUnitCircle * treeSpawnRange;
            Vector3 v = transform.position + new Vector3(rng.x, transform.position.y + 5, rng.y);
            RaycastHit hit;
            bool b = Physics.Raycast(v, Vector3.down, out hit, 10, mask);
            if (b)
            {
                GameObject part = trees[Random.Range(0, trees.Length)];
                GameObject g = Instantiate(part, hit.point, Quaternion.Euler(new Vector3(0, Random.Range(0, 1f) * 360, 0)), transform);
                spawnedParts.Add(g);
            }

            s++;
            if (s % skipFrame == 0)
                yield return new WaitForFixedUpdate();
        }


        r = Random.Range(grassSpawnLimit.x, grassSpawnLimit.y);

        for (int i = 0; i < r; i++)
        {
            Vector2 rng = Random.insideUnitCircle * grassSpawnRange;
            Vector3 v = transform.position + new Vector3(rng.x, transform.position.y + 5, rng.y);
            RaycastHit hit;

            bool b = Physics.Raycast(v, Vector3.down, out hit, 10, mask);
            if (b)
            {
                GameObject part = grasses[Random.Range(0, grasses.Length)];
                GameObject g = Instantiate(part, hit.point, Quaternion.Euler(new Vector3(0, Random.Range(0, 1f) * 360, 0)), transform);
                spawnedParts.Add(g);
            }

            s++;
            if (s % skipFrame == 0)
                yield return new WaitForFixedUpdate();
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
                        StartCoroutine(SpawnParts());
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
