using System;
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LoadLevelScript : MonoBehaviour
{
	public GameObject LoadedLevel;
	public List<GameObject> Levels = new List<GameObject>();

	void OnEnable()
	{
		Levels = Game.Instance.IOManager.GetLevelsInDirectory(Application.dataPath + "/Levels/");
		LoadedLevel = null;
	}
	void Start()
	{
	}
	public void CreateLevel()
	{
		LoadedLevel = Game.Instance.IOManager.Create();
		UpdateLevelListAndSetCurrent(LoadedLevel.GetComponent<LevelBase>().Path);
	}
	public void LoadSelectedLevel()
	{
		Game.Instance.IOManager.Load(LoadedLevel.GetComponent<LevelBase>().Path);
	}
	public void SaveSelectedLevel()
	{
		if(LoadedLevel.GetComponent<LevelBase>().Name != null)
		{
			Game.Instance.IOManager.SaveFromPath(LoadedLevel.GetComponent<LevelBase>().Path);
			return;
		}
		string path = Game.Instance.IOManager.SaveAndGetPath();
		UpdateLevelListAndSetCurrent(path);
	}

	private void UpdateLevelListAndSetCurrent(string path)
	{
		int index = path.LastIndexOf('/');
		path = path.Substring(index + 1);
		Levels = Game.Instance.IOManager.GetLevelsInDirectory(Application.dataPath + "/Levels/");	
		foreach(GameObject l in Levels)
		{
			if(path == l.GetComponent<LevelBase>().Name)
			{
				LoadedLevel = l;
				return;
			}
		}	
	}
}


