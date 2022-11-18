using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TextureGenerator {

	public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height) {
		Texture2D texture = new Texture2D (width, height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels (colourMap);
		texture.Apply ();
		return texture;
	}


	public static Texture2D TextureFromHeightMap(float[,] heightMap) {
		int width = heightMap.GetLength (0);
		int height = heightMap.GetLength (1);

		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, heightMap [x, y]);
			}
		}

		return TextureFromColourMap (colourMap, width, height);
	}

	public static Texture2D TextureFromStructureData(List<StructureGenerator.StructureData> data, int size)
	{
		int width = size;
		int height = size;

		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				colourMap[y * width + x] = Color.black;
			}
		}

        foreach (StructureGenerator.StructureData d in data)
        {
            switch (d.structureType)
            {
                case StructureGenerator.StructureType.MissionPoint:
					colourMap[(int)d.position.y * width + (int)d.position.x] = Color.red;
                    break;
				case StructureGenerator.StructureType.MinorRessourcePoint:
					colourMap[(int)d.position.y * width + (int)d.position.x] = Color.yellow;
                    break;
				case StructureGenerator.StructureType.MajorRessourcePoint:
					colourMap[(int)d.position.y * width + (int)d.position.x] = Color.cyan;
                    break;
				case StructureGenerator.StructureType.Details:
					colourMap[(int)d.position.y * width + (int)d.position.x] = Color.green;
                    break;
				default:
                    break;
            }

		}

		return TextureFromColourMap(colourMap, width, height);
	}

}
