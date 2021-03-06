﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GridCell : MonoBehaviour
{		
	private GameObject m_currentBlock;
	private SerializableVector2 m_dimensions;

	public GridCell() 
	{ 
		m_currentBlock = null;
	}
		
	public void SetBlock(GameObject block, int xCountCells = 0) 
	{ 
		m_currentBlock = block;
		if(xCountCells > 0)
		{
			m_currentBlock.transform.position = transform.position;	

			Sprite s = m_currentBlock.GetComponent<SpriteRenderer>().sprite;
			float windowHeight = Camera.main.orthographicSize * 2f;
			float windowWidth = windowHeight / Screen.height * Screen.width;

			float spriteHeight = s.bounds.size.y;
			float spriteWidth = s.bounds.size.x;

			float scale = (windowWidth / spriteWidth) / xCountCells;
			m_currentBlock.transform.localScale = new Vector2(scale, scale);
		}
	}

	public GameObject GetBlock() 
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

	public bool Inside(float x, float y)
	{
		float halfSizeX = m_dimensions.x * 0.5f;
		float halfSizeY = m_dimensions.y * 0.5f;
		if(x > transform.position.x - halfSizeX && x < transform.position.x + halfSizeX) 
		if(y > transform.position.y - halfSizeY && y < transform.position.y + halfSizeY) 
			return true;
		
		return false;
	}
		
	public GameObject UpdateInput()
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
};
