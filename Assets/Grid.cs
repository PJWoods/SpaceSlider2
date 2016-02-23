using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class Grid : MonoBehaviour 
{
	[System.Serializable]
	public class CellCount
	{
		public int X;
		public int Y;
	};
	public GameObject CellPrefab { get { return m_cellPrefab; } }
	private GameObject m_cellPrefab;

	public CellCount Cells { get { return m_cellCount; } }
	private CellCount m_cellCount;

	public Vector2 CellDimensions { get { return m_cellDimensions; } }
	private Vector2 m_cellDimensions;

	public float GameSpeed { get { return m_gameSpeed; } }
	private float m_gameSpeed;

	private List<GameObject> m_blockPrefabs;
	private List<List<GameObject>> m_cells;
	private BlockBase m_selectedBlock;
	private bool m_isSaved = true;

	void Awake()
	{
		m_cellCount = Cells;
		m_cellDimensions = CellDimensions;

		m_cellPrefab = GameObject.Instantiate(Resources.Load("Prefabs/Levels/GridCell"), Vector3.zero, Quaternion.identity) as GameObject;
		m_blockPrefabs = new List<GameObject>()
		{
			GameObject.Instantiate(Resources.Load("Prefabs/Blocks/EmptyBlock"), Vector3.zero, Quaternion.identity) as GameObject,
			GameObject.Instantiate(Resources.Load("Prefabs/Blocks/Movable"), Vector3.zero, Quaternion.identity) as GameObject,
			GameObject.Instantiate(Resources.Load("Prefabs/Blocks/NonMovable"), Vector3.zero, Quaternion.identity) as GameObject,
			GameObject.Instantiate(Resources.Load("Prefabs/Blocks/LaneChangerLeft"), Vector3.zero, Quaternion.identity) as GameObject,
			GameObject.Instantiate(Resources.Load("Prefabs/Blocks/LaneChangerRight"), Vector3.zero, Quaternion.identity) as GameObject,
			GameObject.Instantiate(Resources.Load("Prefabs/Blocks/SlowPowerUp"), Vector3.zero, Quaternion.identity) as GameObject,
			GameObject.Instantiate(Resources.Load("Prefabs/Blocks/BombPowerUp"), Vector3.zero, Quaternion.identity) as GameObject,
		};
	}

	void Start () 
	{
	}

	public void Init(int width, int height, StreamReader reader = null)
	{
		if(m_cells != null)
		{
			Reset();
			return;
		}

		if(m_cellCount == null)
			m_cellCount = new CellCount();
		
		m_cellCount.X = width;
		m_cellCount.Y = height;

		m_cells = new List<List<GameObject>>(m_cellCount.Y);
		for (int y = 0; y < m_cellCount.Y; ++y) 
		{
			List<GameObject> newColumn = new List<GameObject>(m_cellCount.X);
			for (int x = 0; x < m_cellCount.X; ++x) 
			{
				GameObject cell = GameObject.Instantiate(m_cellPrefab);
				GameObject tile = null;
				if(reader != null)
				{
					int index = int.Parse(reader.ReadLine());
					if(index > 0)
					{
						tile = GameObject.Instantiate(m_blockPrefabs[index]);
					}
				}
				InitCell(cell, tile, x, y);
				newColumn.Add(cell);
			}
			m_cells.Add(newColumn);
		}		
	}
	private void InitCell(GameObject cell, GameObject tile, int x, int y)
	{
		Sprite s = m_cellPrefab.GetComponent<SpriteRenderer>().sprite;
		float windowHeight = Camera.main.orthographicSize * 2f;
		float windowWidth = windowHeight / Screen.height * Screen.width;

		float spriteHeight = s.bounds.size.y;
		float spriteWidth = s.bounds.size.x;

		float scale = (windowWidth / spriteWidth) / m_cellCount.X;
		m_cellDimensions = new Vector2(scale * spriteWidth, scale * spriteWidth);

		Vector3 centerPosition = Vector3.zero;
		float centerX = (Cells.X * CellDimensions.x) * 0.5f;
		float startX = centerPosition.x - centerX;
		float startY = centerPosition.y;

		Vector3 virtualPosition = new Vector3(startX + (CellDimensions.x * 0.5f) + (x * CellDimensions.x), (startY + CellDimensions.y * 0.5f + (y * CellDimensions.y)), 0.3f);
		cell.transform.position = virtualPosition;
		cell.transform.localScale = new Vector2(scale, scale);

		if(tile != null)
		{
			tile.SetActive(true);
			tile.GetComponent<BlockBase>().SetParentCell(cell);
			tile.GetComponent<BlockBase>().SetGridIndex(new Vector2(x, y));
			tile.GetComponent<BlockBase>().SetGrid(this);
			cell.GetComponent<GridCell>().SetBlock(tile, true);		
		}
		cell.SetActive(true);
	}
	//Builds the grid from a list of gameobject, this is made post Init()!
	public void BuildFromData(List<List<GameObject>> list)
	{
		if(m_cells == null)
		{
			Debug.LogError("Grid not inited!");
			return;			
		}
		m_cells = list;			
	}
	//Builds the grid from a level file, this also inits the grid
	public void InitAndLoadLevel(string levelName)
	{
		string path = Application.dataPath + "/Levels/" + levelName + ".lel";

		StreamReader reader = File.OpenText(path);
		m_gameSpeed = int.Parse(reader.ReadLine());
		int mapWidth = int.Parse(reader.ReadLine());
		int mapHeight = int.Parse(reader.ReadLine());

		Init(mapWidth, mapHeight, reader);
		reader.Close();
	}

	void Update ()
	{
		if(m_cells == null) return;

		if(m_selectedBlock != null)
		{
			m_selectedBlock.UpdateMovement();
			if(!Input.GetMouseButton(0))
				m_selectedBlock = null;
		}
	}

	public void SetSelectedBlock(BlockBase block)
	{
		m_selectedBlock = block;
	}

	public void SetIsSaved(bool isSaved)
	{
		m_isSaved = isSaved; 
	}
	public bool GetIsSaved()
	{
		return m_isSaved;
	}

	public void Reset()
	{
		if(m_cells == null) { return; }
		for (int y = 0; y < m_cells.Count; ++y) 
		{
			for (int x = 0; x < m_cells[y].Count; ++x) 
			{
				if(m_cells[y][x])
					Game.Instance.ObjectPool.AddToPool(m_cells[y][x]);				
			}
			m_cells[y].Clear();
		}	
		m_cells.Clear();
	}

	public List<List<GameObject>> GetCells()
	{
		return m_cells;
	}
		
	public GameObject GetCellFromScreenPosition(Vector2 position)
	{
		if(m_cells == null) { return null; }
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, -Camera.main.transform.position.z));
		return GetCellFromWorldPosition(worldPos);
	}

	public GameObject GetCellFromWorldPosition(Vector3 position)
	{
		if(m_cells == null) { return null; }
		Vector3 centerPosition = Vector3.zero;
		float centerX = (Cells.X * CellDimensions.x) * 0.5f;
		float startX = centerPosition.x - centerX;
		float startY = centerPosition.y;

		for (int y = 0; y < m_cells.Count; ++y) 
		{
			for (int x = 0; x < m_cells[y].Count; ++x) 
			{
				Vector3 virtualPosition = new Vector3(startX + CellDimensions.x * 0.5f + (x * CellDimensions.x), (startY + CellDimensions.y * 0.5f + (y * CellDimensions.y)), Camera.main.nearClipPlane);
				float halfSizeX = CellDimensions.x * 0.5f;
				float halfSizeY = CellDimensions.y * 0.5f;
				if(position.x > virtualPosition.x - halfSizeX && position.x < virtualPosition.x + halfSizeX)
				{
					if(position.y > virtualPosition.y - halfSizeY && position.y < virtualPosition.y + halfSizeY)
					{
						return m_cells[y][x];
					}
				}
			}
		}	
		return null;
	}

	public Vector3 GetWorldPositionFromIndex(Vector2 indices)
	{
		Vector3 centerPosition = Vector3.zero;
		float centerX = (Cells.X * CellDimensions.x) * 0.5f;
		float startX = centerPosition.x - centerX;
		float startY = centerPosition.y;
		return new Vector3(startX + CellDimensions.x * 0.5f + (indices.x * CellDimensions.x), (startY + CellDimensions.y * 0.5f + (indices.y * CellDimensions.y)), Camera.main.nearClipPlane);	
	}

	public Vector2 GetCellIndexFromWorldPosition(Vector3 position)
	{
		Vector3 centerPosition = Vector3.zero;
		float centerX = (Cells.X * CellDimensions.x) * 0.5f;
		float startX = centerPosition.x - centerX;
		float startY = centerPosition.y;

		for (int y = 0; y < m_cells.Count; ++y) 
		{
			for (int x = 0; x < m_cells[y].Count; ++x) 
			{
				Vector3 virtualPosition = new Vector3(startX + CellDimensions.x * 0.5f + (x * CellDimensions.x), (startY + CellDimensions.y * 0.5f + (y * CellDimensions.y)), Camera.main.nearClipPlane);
				float halfSizeX = CellDimensions.x * 0.5f;
				float halfSizeY = CellDimensions.y * 0.5f;
				if(position.x > virtualPosition.x - halfSizeX && position.x < virtualPosition.x + halfSizeX)
				{
					if(position.y > virtualPosition.y - halfSizeY && position.y < virtualPosition.y + halfSizeY)
					{
						return new Vector2(x, y);
					}
				}
			}
		}
		return new Vector2(-1, -1);
	}
	public Vector2 GetCellIndexFromScreenPosition(Vector3 position)
	{
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, -Camera.main.transform.position.z));
		return GetCellIndexFromWorldPosition(worldPos);
	}

	public bool IsSameCell(GameObject cellOne, GameObject cellTwo)
	{
		if(cellOne == null || cellTwo == null) return false;
		Vector2 cellIndex1 = GetCellIndexFromWorldPosition(cellOne.transform.position);
		Vector2 cellIndex2 = GetCellIndexFromWorldPosition(cellTwo.transform.position);
		if(cellIndex1.x != cellIndex2.x) return false;
		if(cellIndex1.y != cellIndex2.y) return false;
		return true;
	}

	public bool IsScreenPositionWithinCellArea(Vector2 position)
	{
		Vector2 index = GetCellIndexFromScreenPosition(position);
		if(index.x < 0 || index.y < 0)
			return false;
		return true;
	}
}
