using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class IOManager : MonoBehaviour 
{ 
	private Rect windowRect = new Rect((Screen.width * 0.5f) - 200f, (Screen.height * 0.5f) - 50f, 400f, 100f);
	private bool m_promptUnsavedLevel = false;

	public void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	public List<GameObject> GetLevelsInDirectory(string directoryPath)
	{
		DirectoryInfo info = new DirectoryInfo(directoryPath);
		FileInfo[] fileInfo = info.GetFiles();

		List<GameObject> levels = new List<GameObject>();
		foreach (FileInfo file in fileInfo) 
		{
			if(file.Name.EndsWith(".lel"))
			{
				GameObject levelParent = (GameObject.Instantiate(Resources.Load("Prefabs/Levels/EmptyLevel")) as GameObject);
				LevelBase levelBase = levelParent.GetComponent<LevelBase>();
				levelBase.Name = file.Name; levelBase.Path = file.FullName;
				levels.Add(levelParent);	
			}
		}			
		return levels;
	}

	public void Create()
	{
		string filePath = UnityEditor.EditorUtility.SaveFilePanel("Create new level", "/Assets/Levels/", "newLevel.lel", "lel");
		CreateFromPath(filePath);
	}
	public void CreateFromPath(string filePath)
	{
		string fileName = filePath.Substring(filePath.LastIndexOf('/') + 1);
		if(fileName.Length > 0)
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			if(player == null)
				player = GameObject.Instantiate(Resources.Load("Prefabs/Levels/PlayerPrefab"), Vector3.zero, Quaternion.identity) as GameObject;

			GameObject level = GameObject.FindGameObjectWithTag("Level");
			if(level == null)
				level = GameObject.Instantiate(Resources.Load("Prefabs/Levels/EmptyLevel"), Vector3.zero, Quaternion.identity) as GameObject;

			if(Camera.main.gameObject.GetComponent<CameraMovement>() == null)
				Camera.main.gameObject.AddComponent<CameraMovement>();
			
			LevelBase levelBase = level.GetComponent<LevelBase>();
			levelBase.Name = fileName; levelBase.Path = filePath;
			levelBase.PlayerObject = player;
			levelBase.Init();

			Grid levelGrid = level.GetComponent<Grid>();
			levelGrid.Init();

			MapEditor.Instance.GetComponent<LoadLevelScript>().SetLevel(level);	
		}
	}

	public void Save()
	{
		string filePath = UnityEditor.EditorUtility.SaveFilePanel("Save current level", "/Assets/Levels/", "newLevel.lel", "lel");
		SaveFromPath(filePath);		
	}

	public string SaveAndGetPath()
	{
		string filePath = UnityEditor.EditorUtility.SaveFilePanel("Save current level", "/Assets/Levels/", "newLevel.lel", "lel");
		SaveFromPath(filePath);
		return filePath;
	}

	public void SaveFromPath(string filePath)
	{
		if(filePath == null || filePath == "" || filePath.Length == 0)
		{
			return;
		}
		GameObject level = MapEditor.Instance.GetComponent<LoadLevelScript>().LoadedLevel;
		if(level)
		{
			LevelBase levelBase = level.GetComponent<LevelBase>();
			Grid gridComponent = level.GetComponent<Grid>();
			List<List<GameObject>> cells = gridComponent.GetCells();

			StreamWriter writer = File.CreateText(filePath);
			writer.WriteLine(gridComponent.Cells.X.ToString());
			writer.WriteLine(gridComponent.Cells.Y.ToString());
			writer.WriteLine(gridComponent.CellDimensions.x.ToString());
			writer.WriteLine(gridComponent.CellDimensions.x.ToString());
			writer.WriteLine(levelBase.CameraStart.x.ToString());
			writer.WriteLine(levelBase.CameraStart.y.ToString());
			writer.WriteLine(levelBase.CameraStart.z.ToString());
			writer.WriteLine(levelBase.PlayerStart.x.ToString());
			writer.WriteLine(levelBase.PlayerStart.y.ToString());
			writer.WriteLine(levelBase.PlayerStart.z.ToString());

			for (int y = 0; y < cells.Count; ++y) 
			{
				for (int x = 0; x < cells[y].Count; ++x) 
				{
					if(cells[y][x] != null)
						writer.WriteLine(cells[y][x].name);
					else
						writer.WriteLine("null");
				}
			}
			writer.Close();
			gridComponent.SetIsSaved(true);
			return;
		}
		Debug.LogError("No Level to save!");
	}

	public void Load()
	{
		GameObject level = MapEditor.Instance.GetComponent<LoadLevelScript>().LoadedLevel;
		if(level)
		{
			level.GetComponent<Grid>();
			if(level.GetComponent<Grid>().GetIsSaved() == false)
			{
				m_promptUnsavedLevel = true;
				return;
			}
		}		

		string filePath = UnityEditor.EditorUtility.OpenFilePanel("Load a level", "/Assets/Levels/", "lel");
		if(filePath == null || filePath == "" || filePath.Length == 0)
		{
			return;
		}
		LoadFromPath(filePath);
	}

	public void LoadFromPath(string path)
	{		
		if(!File.Exists(path))
		{
			Debug.LogError("File doesn't exist!");
			return;
		}
		GameObject level = MapEditor.Instance.GetComponent<LoadLevelScript>().LoadedLevel;
		if(!level)
		{
			CreateFromPath(path);
			level = MapEditor.Instance.GetComponent<LoadLevelScript>().LoadedLevel;
		}

		Grid gridComponent = level.GetComponent<Grid>();
		gridComponent.Init();

		LevelBase levelBase = level.GetComponent<LevelBase>();

		StreamReader reader = File.OpenText(path);
		gridComponent.Cells.X = int.Parse(reader.ReadLine());
		gridComponent.Cells.Y = int.Parse(reader.ReadLine());
		gridComponent.CellDimensions.x = float.Parse(reader.ReadLine());
		gridComponent.CellDimensions.y = float.Parse(reader.ReadLine());
		levelBase.CameraStart.x = float.Parse(reader.ReadLine());
		levelBase.CameraStart.y = float.Parse(reader.ReadLine());
		levelBase.CameraStart.z = float.Parse(reader.ReadLine());
		levelBase.PlayerStart.x = float.Parse(reader.ReadLine());
		levelBase.PlayerStart.y = float.Parse(reader.ReadLine());
		levelBase.PlayerStart.z = float.Parse(reader.ReadLine());

		Vector3 centerPosition = Camera.main.transform.position;
		float centerX = (gridComponent.Cells.X * gridComponent.CellDimensions.x) * 0.5f;
		float centerY = (gridComponent.Cells.Y * gridComponent.CellDimensions.y) * 0.5f;
		float startX = Camera.main.transform.position.x;
		float startY = centerPosition.y - centerY;
		if(gridComponent.Type == Grid.GridType.Vertical)
		{
			startX = centerPosition.x - centerX;
			startY = centerPosition.y;			
		}			

		string readLine = "";
		List<List<GameObject>> cells = new List<List<GameObject>>(gridComponent.Cells.Y);
		for (int y = 0; y < gridComponent.Cells.Y; ++y) 
		{
			List<GameObject> newColumn = new List<GameObject>(gridComponent.Cells.X);
			for (int x = 0; x < gridComponent.Cells.X; ++x) 
			{
				readLine = reader.ReadLine();
				if(readLine != "null")
				{
					GameObject block = Game.Instance.ObjectPool.GetFromPool(readLine, true);
					block.transform.position = new Vector3(startX + gridComponent.CellDimensions.x * 0.5f + (x * gridComponent.CellDimensions.x), (startY + gridComponent.CellDimensions.y * 0.5f + (y * gridComponent.CellDimensions.y)), Camera.main.nearClipPlane);
					block.GetComponent<BlockBase>().SetGridIndices(new Vector2(x, y));
					newColumn.Add(block);
				}
			}
			cells.Add(newColumn);
		}	
		gridComponent.BuildFromData(cells);
		levelBase.Init();
	}

	void OnGUI()
	{
		if(m_promptUnsavedLevel)
			windowRect = GUI.Window(0, windowRect, PromptUnsavedLevel, "\nThe current changes haven't been saved.\nWould you like to save them now?");
	}

	void PromptUnsavedLevel(int windowId)
	{
		GUI.FocusWindow(windowId);
		if(GUI.Button(new Rect((windowRect.width * 0.5f) - 100, 60, 80, 20), "Yes"))
		{
			m_promptUnsavedLevel = false;
			Save();
			Load();
		}
		else if(GUI.Button(new Rect((windowRect.width * 0.5f) + 20, 60, 80, 20), "No"))
		{
			m_promptUnsavedLevel = false;
			Load();
			return;			
		}
	}
}
