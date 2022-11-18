using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StructureGenerator))]
public class StructureGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		StructureGenerator mapGen = (StructureGenerator)target;

		if (DrawDefaultInspector())
		{
			if (mapGen.autoUpdate)
			{
				mapGen.DrawMapInEditor();
			}
		}

		if (GUILayout.Button("Generate"))
		{
			mapGen.DrawMapInEditor();
		}
	}
}
