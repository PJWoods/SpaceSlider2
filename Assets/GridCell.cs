using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
////////[ExecuteInEditMode]
public class GridCell
{		
	private BlockBase m_currentBlock;

	private SerializableVector3 m_position;
	private SerializableVector2 m_dimensions;

	public GridCell() 
	{ 
		m_currentBlock = null;
	}

	public void SetBlock(BlockBase block) 
	{ 
		if(m_currentBlock)
		{
			m_currentBlock.SetParentCell(null);
		}

		m_currentBlock = block;
		if(m_currentBlock)
			m_currentBlock.SetParentCell(this);
	}

	public BlockBase GetBlock() 
	{ 
		return m_currentBlock;
	}

	public void SetDimensions(float x, float y) 
	{ 
		m_dimensions.x = x; 
		m_dimensions.y = y; 
	}
	public Vector2 GetDimensions() 
	{ 
		return m_dimensions; 
	}

	public void SetPosition(float x, float y, float z) 
	{ 
		m_position.x = x; 
		m_position.y = y; 
		m_position.z = z; 
	}
	public Vector3 GetPosition() 
	{ 
		return m_position; 
	}

	public bool Inside(float x, float y)
	{
		float halfSizeX = m_dimensions.x * 0.5f;
		float halfSizeY = m_dimensions.x * 0.5f;
		if(x > m_position.x - halfSizeX && x < m_position.x + halfSizeX) 
			if(y > m_position.y - halfSizeY && y < m_position.y + halfSizeY) 
				return true;
		
		return false;
	}
		
	public BlockBase UpdateInput()
	{

			if(Input.GetMouseButton(0))
			{
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
				if(Inside(mousePos.x, mousePos.y))
				{	
					return m_currentBlock;
				}
			}
		return null;
	}
	public void Update()
	{	
		//UpdateInput();
	}

	void PromptBlockType()
	{
	}
};
