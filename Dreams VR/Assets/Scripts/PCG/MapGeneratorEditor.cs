using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor {
	/*
	 * Applies the generated texture to the
	 * target plane in the Unity editor mode.
	 * This is just to make testing easier.
	 */
	public override void OnInspectorGUI(){
		MapGenerator mapGen = (MapGenerator)target;

		if (DrawDefaultInspector ()) {
			if (mapGen.autoUpdate)
				mapGen.GenerateMap ();
		}

		if (GUILayout.Button ("Generate")) {
			mapGen.GenerateMap ();
		}
	}
}
