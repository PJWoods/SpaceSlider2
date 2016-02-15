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
		PowerUp,
		LaneChangerLeft,
		LaneChangerRight,
		TotalAmountOfTypes
	};

	public BlockProperty 	BlockType;
	protected Vector2 		m_gridIndex;
	protected Grid 			m_grid;

	void Awake () 
	{
		DontDestroyOnLoad(gameObject);
	
		GameObject level = GameState.CurrentLevel;
		if(level)
		{
			m_grid = level.GetComponent<Grid>();
		}
		else
		{
			Debug.LogError("Couldnt find object with tag 'Level'!");
		}
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
	}

	void OnMouseDown()
	{
		if(!MapEditor.Instance)
		{	
			m_grid.SetSelectedBlock(this);
		}	
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
