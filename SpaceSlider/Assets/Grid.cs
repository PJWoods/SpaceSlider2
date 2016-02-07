using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
////////[ExecuteInEditMode]
public class Grid : MonoBehaviour {

	[System.Serializable]
	public class CellCount
	{
		public int X;
		public int Y;
	};

	public bool ShowDebugLines = false;
	public bool ShowDebugPositions = false;
	public CellCount Cells;
	public Vector2 CellDimensions;

	private List<List<GridCell>> m_cells;
	private bool m_isSaved = true;
	private BlockBase m_selectedBlock;

	void Awake()
	{

	}
	void OnEnable()
	{
		Debug.Assert(Cells.X > 0 && Cells.Y > 0 && CellDimensions.x > 0 && CellDimensions.y > 0, "INVALID GRID! Gör om gör rätt!");

		Vector3 centerPosition = Camera.main.transform.position;
		//float centerX = (Cells.X * CellDimensions.x) * 0.5f;
		float centerY = (Cells.Y * CellDimensions.y) * 0.5f;
		float startX = centerPosition.x;//centerPosition.x - centerX;
		float startY = centerPosition.y - centerY;

		m_cells = new List<List<GridCell>>(Cells.Y);
		for (int y = 0; y < Cells.Y; ++y) 
		{
			List<GridCell> newColumn = new List<GridCell>(Cells.X);
			for (int x = 0; x < Cells.X; ++x) 
			{
				GridCell cell = new GridCell();
				cell.SetDimensions(CellDimensions.x, CellDimensions.y);
				cell.SetPosition(startX + CellDimensions.x * 0.5f + (x * CellDimensions.x), (startY + CellDimensions.y * 0.5f + (y * CellDimensions.y)), Camera.main.nearClipPlane);
				newColumn.Add(cell);
			}
			m_cells.Add(newColumn);
		}			
	}
	void Start () 
	{

	}

	void Update ()
	{
		if(!MapEditor.Instance)
		{
			if(m_cells == null) return;

			if(m_selectedBlock == null)
			{
				for (int y = 0; y < m_cells.Count; ++y) 
				{
					for (int x = 0; x < m_cells[y].Count; ++x) 
					{
						m_selectedBlock = m_cells[y][x].UpdateInput();
						if(m_selectedBlock)
							return;
					}
				}
			}
			else
			{
				m_selectedBlock.UpdateMovement();
				if(!Input.GetMouseButton(0))
					m_selectedBlock = null;
			}
		}
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
				GridCell cell = m_cells[y][x];
				if(cell.GetBlock() != null)
				{
					GameObjectPool.Instance.AddToPool(cell.GetBlock().gameObject);				
				}
			}
			m_cells[y].Clear();
		}	
		m_cells.Clear();
	}

	public void SetCells(List<List<GridCell>> cells)
	{
		m_cells = cells;
	}

	public List<List<GridCell>> GetCells()
	{
		return m_cells;
	}

	public GridCell GetCellFromScreenPosition(Vector2 position)
	{
		if(m_cells == null) { return null; }
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, -Camera.main.transform.position.z));
		for (int y = 0; y < m_cells.Count; ++y) 
		{
			for (int x = 0; x < m_cells[y].Count; ++x) 
			{
				if(m_cells[y][x].Inside(worldPos.x, worldPos.y))
					return m_cells[y][x];
			}
		}	
		return null;
	}

	public GridCell GetCellFromWorldPosition(Vector3 position)
	{
		if(m_cells == null) { return null; }
		for (int y = 0; y < m_cells.Count; ++y) 
		{
			for (int x = 0; x < m_cells[y].Count; ++x) 
			{
				if(m_cells[y][x].Inside(position.x, position.y))
					return m_cells[y][x];
			}
		}	
		return null;
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
					GridCell cell = m_cells[y][x];
					Gizmos.DrawWireCube(cell.GetPosition(), new Vector3(CellDimensions.x, CellDimensions.y, 0f));
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
					GridCell cell = m_cells[y][x];
					Vector3 worldPos = new Vector3(cell.GetPosition().x, cell.GetPosition().y + 0.8f, cell.GetPosition().z);
					Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
					Rect screenRectangle = new Rect(screenPos.x - 18.0f, Screen.height - screenPos.y , 50.0f, 60.0f);

					string text = "x: " + cell.GetPosition().x.ToString() + "\ny: " + cell.GetPosition().y.ToString() + "\nz: " + cell.GetPosition().z.ToString();
					GUI.Label(screenRectangle, text);
				}
			}	
		}
	}
}
