using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
////////[ExecuteInEditMode]
public class Grid : MonoBehaviour {

	public enum GridType
	{
		Horizontal,
		Vertical
	};

	[System.Serializable]
	public class CellCount
	{
		public int X;
		public int Y;
	};

	public bool ShowDebugLines = false;
	public bool ShowDebugPositions = false;
	public GridType Type;
	public CellCount Cells;
	public Vector2 CellDimensions;

	[SerializeField]
	private GameObject CellPrefab;

	private List<List<GameObject>> m_cells;
	private BlockBase m_selectedBlock;
	private bool m_isSaved = true;

	void Awake()
	{
		if(Cells.X <= 0 || Cells.Y <= 0 || CellDimensions.x <= 0 || CellDimensions.y <= 0)
		{
			Debug.LogError("INVALID GRID!");
			return;			
		}
	}

	void Start () 
	{
	}

	public void Init()
	{
		if(m_cells != null)
		{
			Reset();
			return;
		}
		m_cells = new List<List<GameObject>>(Cells.Y);
		for (int y = 0; y < Cells.Y; ++y) 
		{
			List<GameObject> newColumn = new List<GameObject>(Cells.X);
			for (int x = 0; x < Cells.X; ++x) 
			{
				newColumn.Add(null);
			}
			m_cells.Add(newColumn);
		}		
	}

	public void BuildFromData(List<List<GameObject>> list)
	{
		if(m_cells == null)
		{
			Debug.LogError("Grid not inited!!");
			return;			
		}
		m_cells = list;			
	}

	void Update ()
	{
		if(!MapEditor.Instance)
		{
			if(m_cells == null) return;

			if(m_selectedBlock != null)
			{
				m_selectedBlock.UpdateMovement();
				if(!Input.GetMouseButton(0))
					m_selectedBlock = null;
			}
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
		Vector3 centerPosition = GetComponent<LevelBase>().CameraStart;
		float centerX = (Cells.X * CellDimensions.x) * 0.5f;
		float centerY = (Cells.Y * CellDimensions.y) * 0.5f;
		float startX = centerPosition.x;
		float startY = centerPosition.y - centerY;
		if(Type == Grid.GridType.Vertical)
		{
			startX = centerPosition.x - centerX;
			startY = centerPosition.y;			
		}

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
	public void AddCellAtScreenPosition(GameObject cell, Vector3 position)
	{
		if(m_cells == null) { return; }
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, -Camera.main.transform.position.z));
		AddCellAtWorldPosition(cell, worldPos);	
	}
	public void AddCellAtWorldPosition(GameObject cell, Vector3 position)
	{
		if(m_cells == null) { return; }
		Vector3 centerPosition = GetComponent<LevelBase>().CameraStart;
		float centerX = (Cells.X * CellDimensions.x) * 0.5f;
		float centerY = (Cells.Y * CellDimensions.y) * 0.5f;
		float startX = centerPosition.x;
		float startY = centerPosition.y - centerY;
		if(Type == Grid.GridType.Vertical)
		{
			startX = centerPosition.x - centerX;
			startY = centerPosition.y;			
		}

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
						if(cell)
						{
							cell.transform.position = virtualPosition;
							cell.GetComponent<BlockBase>().SetGridIndices(new Vector2(x, y));
						}
						m_cells[y][x] = cell;
						return;
					}
				}
			}
		}		
	}

	public Vector3 GetWorldPositionFromIndex(Vector2 indices)
	{
		Vector3 centerPosition = GetComponent<LevelBase>().CameraStart;
		float centerX = (Cells.X * CellDimensions.x) * 0.5f;
		float centerY = (Cells.Y * CellDimensions.y) * 0.5f;
		float startX = centerPosition.x;
		float startY = centerPosition.y - centerY;
		if(Type == Grid.GridType.Vertical)
		{
			startX = centerPosition.x - centerX;
			startY = centerPosition.y;			
		}
		return new Vector3(startX + CellDimensions.x * 0.5f + (indices.x * CellDimensions.x), (startY + CellDimensions.y * 0.5f + (indices.y * CellDimensions.y)), Camera.main.nearClipPlane);	
	}

	public Vector2 GetCellIndexFromWorldPosition(Vector3 position)
	{
		Vector3 centerPosition = GetComponent<LevelBase>().CameraStart;
		float centerX = (Cells.X * CellDimensions.x) * 0.5f;
		float centerY = (Cells.Y * CellDimensions.y) * 0.5f;
		float startX = centerPosition.x;
		float startY = centerPosition.y - centerY;
		if(Type == Grid.GridType.Vertical)
		{
			startX = centerPosition.x - centerX;
			startY = centerPosition.y;			
		}

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
	void OnDrawGizmos()
	{
		if(ShowDebugLines)
		{
			if(m_cells == null) { return; }

			for (int y = 0; y < m_cells.Count; ++y) 
			{
				for (int x = 0; x < m_cells[y].Count; ++x) 
				{
					Vector3 pos = GetWorldPositionFromIndex(new Vector2(x, y));
					Gizmos.DrawWireCube(pos, new Vector3(CellDimensions.x, CellDimensions.y, 0f));
				}
			}	
		}		
	}

	void OnGUI()
	{
		if(ShowDebugPositions)
		{
			if(m_cells == null) { return; }
			for (int y = 0; y < m_cells.Count; ++y) 
			{
				for (int x = 0; x < m_cells[y].Count; ++x) 
				{
					GameObject cell = m_cells[y][x];
					if(cell == null) continue;

					Vector3 worldPos = new Vector3(cell.transform.position.x, cell.transform.position.y + 0.8f, cell.transform.position.z);
					Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
					Rect screenRectangle = new Rect(screenPos.x - 18.0f, Screen.height - screenPos.y , 50.0f, 60.0f);

					string text = "x: " + cell.transform.position.x.ToString() + "\ny: " + cell.transform.position.y.ToString() + "\nz: " + cell.transform.position.z.ToString();
					GUI.Label(screenRectangle, text);
				}
			}	
		}
	}
}
