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
		LaneChangerLeft,
		LaneChangerRight,
		LaneChangerSwipe,
		SlowPowerUp,
		BombPowerUp,
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
	public void Start () 
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

	public virtual void OnCollision()
	{
	}

	public virtual void UpdateMovement()
	{
	}

	// Update is called once per frame
	protected virtual void Update () 
	{
		ShaderEffectScript effect = GetComponent<ShaderEffectScript>();
		if(!effect)
			return;
		effect.Update();

	}

	protected virtual void OnMouseDown()
	{
		m_grid.SetSelectedBlock(this);
	}


}
