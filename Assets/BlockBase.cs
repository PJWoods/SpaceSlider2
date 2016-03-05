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

	[SerializeField]
	protected Vector4		m_shineColor;
	[SerializeField]
	protected float			m_shineEffectWidth = 0.1f;
	[SerializeField]
	protected float			m_shineColorWidth = 0.5f;
	[SerializeField]
	protected float			m_shineColorFill = 0.5f;
	[SerializeField]
	protected float			m_shineSpeed = 0.5f;

	private Material 		m_material;
	private float			m_shineLocation;

	void Awake ()
	{
		
	}

	// Use this for initialization
	public void Start () 
	{
		m_material = GetComponent<SpriteRenderer>().material;
		if(m_material)
		{
			m_material.SetVector("_Color", m_shineColor);
			m_material.SetFloat("_EffectWidth", m_shineEffectWidth);
			m_material.SetFloat("_ShineWidth", m_shineColorWidth);
			m_material.SetFloat("_ColorFill", m_shineColorFill);	
		}
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
		if(!m_material)
			return;
		
		m_shineLocation += Time.deltaTime * m_shineSpeed;
		if(m_shineLocation >= 2f)
		{
			m_shineLocation = -1f;
		}
		m_material.SetFloat("_ShineLocation", m_shineLocation);	
	}

	protected virtual void OnMouseDown()
	{
		m_grid.SetSelectedBlock(this);
	}


}
