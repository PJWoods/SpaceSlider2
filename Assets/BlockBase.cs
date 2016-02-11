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
	protected GridCell 		m_parentCell;

	void Awake () 
	{
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
	}

	public void SetParentCell(GridCell parentCell)
	{
		m_parentCell = parentCell;
	}

	public virtual void OnCollision()
	{
	}

	public virtual void UpdateMovement()
	{
	}
}
