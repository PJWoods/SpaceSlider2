using System;
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LoadLevelScript : MonoBehaviour
{
	[Serializable]
	public class Level
	{
		[ShowOnly] public string Name;
		[ShowOnly] public string Path;
		public Level(string name, string path)
		{
			Name = name;
			Path = path;
		}
	};

	public Level LoadedLevel;
	public List<Level> Levels = new List<Level>();

	void OnEnable()
	{
		Levels = Game.Instance.IOManager.GetLevelsInDirectory(Application.dataPath + "/Levels/");		
		LoadedLevel = null;
	}
	void Start()
	{

	}

	public void LoadSelectedLevel()
	{
		Game.Instance.IOManager.Load(LoadedLevel.Path);
	}
	public void SaveSelectedLevel()
	{
		if(LoadedLevel.Name != null)
		{
			Game.Instance.IOManager.SaveFromPath(LoadedLevel.Path);
			return;
		}
		string path = Game.Instance.IOManager.SaveAndGetPath();
		int index = path.LastIndexOf('/');
		path = path.Substring(index + 1);
		Levels = Game.Instance.IOManager.GetLevelsInDirectory(Application.dataPath + "/Levels/");	
		foreach(Level l in Levels)
		{
			if(path == l.Name)
			{
				LoadedLevel = l;
				return;
			}
		}
	}
}


