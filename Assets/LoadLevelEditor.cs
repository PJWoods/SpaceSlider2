using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;

[CustomEditor(typeof(LoadLevelScript))]
public class LoadLevelEditor : Editor
{
	void OnEnable()
	{
	}
	public override void OnInspectorGUI()
	{
		LoadLevelScript script = (LoadLevelScript)target;
		string name = "NONE";
		if(script.LoadedLevel != null)
		{
			name = script.LoadedLevel.GetComponent<LevelBase>().Name;			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Loaded level: [" + name + "]");
			if(Application.isPlaying && script.LoadedLevel.GetComponent<Grid>().GetIsSaved() == false)
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
		}
		else
		{	
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Loaded level: [" + name + "]");
			if(Application.isPlaying)
			{
				if(GUILayout.Button("Create"))
				{
					script.CreateLevel();					
				}				
			}
			EditorGUILayout.EndHorizontal();			
		}

		List<GameObject> listOfLevels = new List<GameObject>(script.Levels);
		EditorGUILayout.LabelField("Available levels: ");

		EditorGUI.indentLevel = 2;
		for (int i = 0; i < listOfLevels.Count; ++i) 
		{
			if(listOfLevels[i])
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(listOfLevels[i].GetComponent<LevelBase>().Name);
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
		}
		EditorUtility.SetDirty(script);
	}
}


