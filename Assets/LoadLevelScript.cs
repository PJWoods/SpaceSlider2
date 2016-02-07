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
	private IOManager m_ioManager;

	void OnEnable()
	{
		GameObject managerObject = GameObject.Find("IOManager");
		m_ioManager = managerObject.GetComponent<IOManager>();
		Levels = m_ioManager.GetLevelsInDirectory(Application.dataPath + "/Levels/");		
		LoadedLevel = null;
	}
	void Start()
	{

	}

	public void LoadSelectedLevel()
	{
		m_ioManager.Load(LoadedLevel.Path);
	}
	public void SaveSelectedLevel()
	{
		if(LoadedLevel.Name != null)
		{
			m_ioManager.SaveFromPath(LoadedLevel.Path);
			return;
		}
		string path = m_ioManager.SaveAndGetPath();
		int index = path.LastIndexOf('/');
		path = path.Substring(index + 1);
		Levels = m_ioManager.GetLevelsInDirectory(Application.dataPath + "/Levels/");	
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


