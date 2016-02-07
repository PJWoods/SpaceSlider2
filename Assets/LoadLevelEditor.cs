using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;

[CustomEditor(typeof(LoadLevelScript))]
public class LoadLevelEditor : Editor
{
	private Grid m_grid;
	void OnEnable()
	{
		m_grid = GameObject.Find("Grid").GetComponent<Grid>();
	}
	public override void OnInspectorGUI()
	{
		LoadLevelScript script = (LoadLevelScript)target;
		string name = "NONE";
		if(script.LoadedLevel.Name != null)
			name = script.LoadedLevel.Name;	
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Loaded level: [" + name + "]");
		if(Application.isPlaying && m_grid.GetIsSaved() == false)
		{
			if(GUILayout.Button("Save"))
			{
				if(script.LoadedLevel != null)
				{
					script.SaveSelectedLevel();					
				}
			}				
		}
		EditorGUILayout.EndHorizontal();

		List<LoadLevelScript.Level> listOfLevels = new List<LoadLevelScript.Level>(script.Levels);
		EditorGUILayout.LabelField("Available levels: ");

		EditorGUI.indentLevel = 2;
		for (int i = 0; i < listOfLevels.Count; ++i) 
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(listOfLevels[i].Name);
			if(Application.isPlaying)
			{
				if(GUILayout.Button("Load"))
				{
					script.LoadedLevel = listOfLevels[i];
					script.LoadSelectedLevel();
				}				
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorUtility.SetDirty(script);
	}
}


