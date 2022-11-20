using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureGenerator : MonoBehaviour
{
	public Noise.NormalizeMode normalizeMode;
    public bool autoUpdate = false;
    public bool preview = false;

    public int mapSize = 350;
    public int seed = 0;

    public float waterLevel = 0;

    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public Vector2 offset;

    public int density;

    public Transform parentStruct;
    public Structure[] structres;

    public void DrawMapInEditor()
    {
        GenerateStrucutreMap();
    }

    public void GenerateStrucutreMap()
    {
        if (parentStruct)
            DestroyImmediate(parentStruct);

        parentStruct = new GameObject("parentStruct").transform;

        float[,] noiseMap = Noise.GenerateNoiseMap(mapSize, mapSize, seed, noiseScale, octaves, persistance, lacunarity, offset, normalizeMode);
        List<StructureData> result = GenerateStructrePlacements(noiseMap);

        if (preview)
        {
            MapDisplay display = FindObjectOfType<MapDisplay>();
            display.DrawTexture(TextureGenerator.TextureFromStructureData(result, mapSize));
        }

        StartCoroutine(SpawnStructres(result));
    }

    IEnumerator SpawnStructres(List<StructureData> result)
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        for (int i = 0; i < result.Count; i++)
        {
            for (int j = 0; j < structres.Length; j++)
            {
                if (result[i].structureType == structres[j].structureType)
                {
                    RaycastHit hit;
                    Physics.Raycast(new Vector3(result[i].position.x, 60, result[i].position.y), Vector3.down, out hit, 100);

                    if (waterLevel < hit.point.y)
                    {
                        int inx = Random.Range(0, structres[j].prefabs.Length);
                        GameObject g = Instantiate(structres[j].prefabs[inx], parentStruct);

                        g.transform.position = new Vector3(result[i].position.x, hit.point.y, result[i].position.y);
                        Vector3 frwd = hit.normal;
                        frwd.y = 0;

                        Vector3 nrml = Vector3.Cross(frwd.normalized, hit.normal);

                        g.transform.rotation = Quaternion.LookRotation(nrml.normalized , hit.normal);

                        result[i].gameObject = g;
                    }
                }
            }
        }

        GameController.Instance.WorldGenFinished();
    }

    public List<StructureData> GenerateStructrePlacements(float[,] noiseMap)
    {
        int height = noiseMap.GetLength(0);
        int width = noiseMap.GetLength(1);

        List<StructureData> structures = new List<StructureData>();

       int densityCount = 0;

        for (int yc = 0; yc < height; yc++)
        {
            for (int xc = 0; xc < width; xc++)
            {
                float max = 0;
                for (int yn = yc - density; yn <= yc + density; yn++)
                {
                    for (int xn = xc - density; xn <= xc + density; xn++)
                    {
                        if (0 <= yn && yn < height && 0 <= xn && xn < width)
                        {
                            float e = noiseMap[yn,xn];
                            if (e > max) { max = e; }
                        }
                    }
                }

                if (noiseMap[yc,xc] == max)
                {
                    densityCount++;
                    foreach (Structure s in structres)
                    {
                        if (densityCount % s.ignoreDensity == 0)
                        {
                            structures.Add(new StructureData() { position = new Vector2((xc * 2) - mapSize, (yc * 2) - mapSize), structureType = s.structureType }) ;
                            break;
                        }
                    }
                }
            }
        }


        return structures;
    }


    public enum StructureType
    {
        MissionPoint,
        MinorRessourcePoint,
        MajorRessourcePoint,
        Details
    }

    [System.Serializable]
    public class Structure
    {
        public StructureType structureType;
        [Range(1,100)]
        public int ignoreDensity;
        public GameObject[] prefabs;
    }

    public class StructureData
    {
        public Vector2 position;
        public StructureType structureType;
        public GameObject gameObject;
    }
}
