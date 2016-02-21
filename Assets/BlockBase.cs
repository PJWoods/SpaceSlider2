using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BlockBase : MonoBehaviour 
{
	[System.Serializable]
	public enum BlockProperty
	{
		Empty,
		Movable,
		NonMovable,
		SlowPowerUp,
		LaneChangerLeft,
		LaneChangerRight,
		TotalAmountOfTypes
	};

	public BlockProperty 	BlockType;
	protected GameObject 	m_parentCell;
	protected Vector2 		m_gridIndex;
	protected Grid 			m_grid;

	void Awake ()
	{
	}

	// Use this for initialization
	void Start () 
	{
	}

	public void SetGrid(Grid grid)
	{
		m_grid = grid;
	}

	public void SetParentCell(GameObject parent)
	{
		m_parentCell = parent;
	}

	public void SetGridIndex(Vector2 gridIndex)
	{
		m_gridIndex = gridIndex;
	}

	// Update is called once per frame
	protected virtual void Update () 
	{
	}

	void OnMouseDown()
	{
		m_grid.SetSelectedBlock(this);
	}

	public virtual void OnCollision()
	{
	}

	public virtual void UpdateMovement()
	{
	}

	public void SetGridIndices(Vector2 indices)
	{
		m_gridIndex = indices;
	}
}
