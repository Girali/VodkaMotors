using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnnemySpawner : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> spawns = new List<GameObject>();
    public int totalDead = 0;
    public float spawnDistance;
    public Vector2Int ennemiesToSpawn;
    private GameObject player;
    public bool spawned;
    public float maxDistance;
    public bool during = false;

    private int layerMask;

    public UnityAction allDead;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Floor");
    }

    public void Death(GameObject g)
    {
        totalDead++;

        if(totalDead == spawns.Count)
            if(allDead != null)
            allDead.Invoke();
    }

    private void Update()
    {
        if (during)
        {
            if (player != null)
            {
                Vector3 v = player.transform.position - transform.position;
                if (maxDistance < v.magnitude)
                {
                    foreach (GameObject g in spawns)
                    {
                        if (g != null)
                            g.SetActive(false);
                    }
                    player = null;
                    during = false;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!during)
        {
            GameObject player = other.gameObject;

            if (player != null && player.tag == "Player")
            {
                this.player = other.attachedRigidbody.gameObject;

                if (spawned)
                {
                    foreach (GameObject g in spawns)
                    {

                        if (g != null)
                        {
                            Vector2 l = Random.insideUnitCircle * spawnDistance;

                            RaycastHit hit;
                            Physics.Raycast(transform.position + new Vector3(l.x, 30, l.y), Vector3.down, out hit, 50f, layerMask);

                            Vector3 v = hit.point;
                            Vector3 r = v - transform.position;
                            r.y = 0;
                            r.Normalize();

                            g.transform.position = v;
                            g.transform.rotation = Quaternion.LookRotation(-r);

                            g.SetActive(true);
                        }
                    }
                }
                else
                {
                    int si = Random.Range(ennemiesToSpawn.x, ennemiesToSpawn.y);
                    for (int i = 0; i < si; i++)
                    {
                        Vector2 l = Random.insideUnitCircle * spawnDistance;

                        RaycastHit hit;
                        Physics.Raycast(transform.position + new Vector3(l.x, 30, l.y), Vector3.down, out hit, 50f, layerMask);

                        Vector3 v = hit.point;
                        Vector3 r = v - transform.position;
                        r.y = 0;
                        r.Normalize();

                        GameObject g = Instantiate(prefab, v, Quaternion.LookRotation(-r));
                        g.GetComponent<EnnemyMotor>().AddSpawner(this);
                        spawns.Add(g);
                    }
                }

                spawned = true;
                during = true;
            }
        }
    }
}
