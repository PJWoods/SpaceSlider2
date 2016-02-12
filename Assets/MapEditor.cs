﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

////////[ExecuteInEditMode]
public class MapEditor : MonoBehaviour {

	public enum EditorDrawMode
	{
		DontDraw,
		DrawMovable,
		DrawNonMovable,
		DrawBoost,
		DrawLaneChangerLeft,
		DrawLaneChangerRight,
		DrawEmpty,
		Total
	}
		
	public static MapEditor Instance { get; private set; }
	private bool m_menuOpen = false;
	private Rect m_menuRect;

	private EditorDrawMode m_drawMode = EditorDrawMode.DontDraw;
	private GameObject m_lastDrawnCell = null;
	private GameObject m_markedCell = null;
	private Vector2 m_clickPos;

	private bool m_active = true;

	void OnEnable()
	{
		Instance = this;
		for(int i = 0; i < transform.childCount; ++i)
			transform.GetChild(i).gameObject.SetActive(true);
	}
	void OnDisable()
	{
		Instance = null;
		for(int i = 0; i < transform.childCount; ++i)
			transform.GetChild(i).gameObject.SetActive(false);
	}

	void Start () 
	{
	}

	// Update is called once per frame
	void Update () 
	{
		CheckAttachmentInput();
		if(!m_active)
			return;
		
//		if(m_drawMode == EditorDrawMode.DontDraw)
//		{
//			if(Input.GetMouseButtonDown(0))
//			{
//				if(m_menuOpen && !m_menuRect.Contains(new Vector2(Input.mousePosition.x, (Screen.height - Input.mousePosition.y))))
//				{
//					m_markedCell = null;
//					m_menuOpen = false;
//					return;
//				}
//				if(Input.mousePosition.y > Screen.height - (Screen.height * 0.08f))
//				{
//					m_markedCell = null;
//					m_menuOpen = false;
//					return;
//				}
//
//				GameObject loadedLevel = GetComponent<LoadLevelScript>().LoadedLevel;
//				if(loadedLevel)
//				{
//					Grid gridComponent = loadedLevel.GetComponent<Grid>();
//					Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
//					GameObject cell = gridComponent.GetCellFromScreenPosition(screenPos);
//					if(!m_menuOpen)
//					{
//						m_markedCell = cell;
//						m_clickPos = screenPos;
//						OpenBlockClickMenu(screenPos);						
//					}						
//				}
//			}
//		}
//		else
		{
			if(Input.GetMouseButton(0))
			{			
				if(Input.mousePosition.y > Screen.height - (Screen.height * 0.08f))
				{
					return;
				}

				GameObject loadedLevel = GetComponent<LoadLevelScript>().LoadedLevel;
				if(loadedLevel)
				{
					Grid gridComponent = loadedLevel.GetComponent<Grid>();
					Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
					GameObject cell = gridComponent.GetCellFromScreenPosition(screenPos);
					if(cell == null)
					{
						m_lastDrawnCell = cell;

						BlockBase block = null;
						switch(m_drawMode)
						{
						case EditorDrawMode.DrawEmpty:
							break;
						case EditorDrawMode.DrawMovable:
							block = BlockFactory.Instance.CreateBlock(BlockBase.BlockProperty.Movable);
							break;
						case EditorDrawMode.DrawNonMovable:
							block = BlockFactory.Instance.CreateBlock(BlockBase.BlockProperty.NonMovable);
							break;
						case EditorDrawMode.DrawBoost:
							block = BlockFactory.Instance.CreateBlock(BlockBase.BlockProperty.PowerUp);
							break;
						case EditorDrawMode.DrawLaneChangerLeft:
							block = BlockFactory.Instance.CreateBlock(BlockBase.BlockProperty.LaneChangerLeft);
							break;
						case EditorDrawMode.DrawLaneChangerRight:
							block = BlockFactory.Instance.CreateBlock(BlockBase.BlockProperty.LaneChangerRight);
							break;
						}
						if(block != null)
						{
							gridComponent.AddCellAtScreenPosition(block.gameObject, screenPos);
							gridComponent.SetIsSaved(false);							
						}
					}
					else
					{
						if(cell != m_lastDrawnCell)
						{
							m_lastDrawnCell = cell;

							BlockBase block = null;
							switch(m_drawMode)
							{
							case EditorDrawMode.DrawEmpty:
								break;
							case EditorDrawMode.DrawMovable:
								block = BlockFactory.Instance.CreateBlock(BlockBase.BlockProperty.Movable);
								break;
							case EditorDrawMode.DrawNonMovable:
								block = BlockFactory.Instance.CreateBlock(BlockBase.BlockProperty.NonMovable);
								break;
							case EditorDrawMode.DrawBoost:
								block = BlockFactory.Instance.CreateBlock(BlockBase.BlockProperty.PowerUp);
								break;
							case EditorDrawMode.DrawLaneChangerLeft:
								block = BlockFactory.Instance.CreateBlock(BlockBase.BlockProperty.LaneChangerLeft);
								break;
							case EditorDrawMode.DrawLaneChangerRight:
								block = BlockFactory.Instance.CreateBlock(BlockBase.BlockProperty.LaneChangerRight);
								break;
							}

							Vector3 cellPos = cell.transform.position;
							Game.Instance.ObjectPool.AddToPool(cell);	
							if(block != null)
								gridComponent.AddCellAtWorldPosition(block.gameObject, cellPos);
							else
								gridComponent.AddCellAtWorldPosition(null, cellPos);
							
							gridComponent.SetIsSaved(false);
						}
					}
				}
			}
		}
	}

	private void CheckAttachmentInput()
	{
		if(!m_active)
		{
			if(Input.GetKeyDown(KeyCode.F5))
			{			
				m_active = true;
				OnEnable();
			}
			return;
		}
		if(Input.GetKeyDown(KeyCode.F5))
		{			
			m_active = false;
			OnDisable();
		}
	}

	void OnGUI()
	{
		if(!Instance)
		{
			return;
		}
		if(m_menuOpen)
			m_menuRect = GUI.Window(0, m_menuRect, ShowRightClickMenu, "\nBlockTypes:\n");
	}

	void ShowRightClickMenu(int windowId)
	{	
		float spacing = 20f;
		float boxWidth = 140f;
		for (int i = 0; i < (int)BlockBase.BlockProperty.TotalAmountOfTypes; ++i) 
		{
			if(GUI.Button(new Rect((m_menuRect.width * 0.5f) - 70f, 40f + (spacing * i), boxWidth, 20), ((BlockBase.BlockProperty)i).ToString()))
			{
				GameObject loadedLevel = GetComponent<LoadLevelScript>().LoadedLevel;
				if(loadedLevel)
				{
					Grid gridComponent = loadedLevel.GetComponent<Grid>();

					Vector3 cellPos = Vector3.zero;
					if(m_markedCell != null)
					{
						cellPos = m_markedCell.transform.position;
						Game.Instance.ObjectPool.AddToPool(m_markedCell);			
					}
					else
					{
						cellPos = m_clickPos;
					}
					BlockBase block = BlockFactory.Instance.CreateBlock(((BlockBase.BlockProperty)i));
					if(block != null)
					{
						gridComponent.AddCellAtScreenPosition(block.gameObject, cellPos);		
						gridComponent.SetIsSaved(false);
					}
					else
					{
						Debug.LogError("Something went wrong when creating block!");
					}
					m_menuOpen = false;
					m_markedCell = null;
					return;
				}
			}				
		}
		if(GUI.Button(new Rect((m_menuRect.width * 0.5f) - 70, 40 + (spacing * (int)BlockBase.BlockProperty.TotalAmountOfTypes), 140, 20), "Cancel"))
		{
			m_menuOpen = false;
		}
	}

	public void OpenBlockClickMenu(Vector2 position)
	{
		m_menuOpen = true;
		float spacing = 20f;
		float boxHeight = ((int)BlockBase.BlockProperty.TotalAmountOfTypes + 1) * 20f + 45f;
		float boxWidth = 140f;

		Vector2 screenPos2 = position;
		screenPos2.x += boxWidth;
		screenPos2.y -= boxHeight;
		if(screenPos2.x > Screen.width) 
			position.x -= (screenPos2.x - Screen.width);
		if(screenPos2.y < 0) 
			position.y -= screenPos2.y;

		m_menuRect = new Rect(position.x, (Screen.height - position.y), 150f, ((int)BlockBase.BlockProperty.TotalAmountOfTypes + 1) * spacing + 45f);
		StartCoroutine(OnLeftClickGridCell());
	}

	IEnumerator OnLeftClickGridCell()
	{
		while(m_menuOpen)
		{
			yield return null;
		}
		yield return false;
	}

	//Draw Mode Toggeling
	public void ToggleDrawMode(int mode)
	{
		m_drawMode = (EditorDrawMode)mode;
	}

	public void ToggleDrawEmpty(bool flag)
	{
		if(flag)
			m_drawMode = EditorDrawMode.DrawEmpty;
	}
	public void ToggleDrawMovable(bool flag)
	{
		if(flag)
			m_drawMode = EditorDrawMode.DrawMovable;
	}
	public void ToggleDrawNonMovable(bool flag)
	{
		if(flag)
			m_drawMode = EditorDrawMode.DrawNonMovable;
	}
	public void ToggleDrawBoost(bool flag)
	{
		if(flag)
			m_drawMode = EditorDrawMode.DrawBoost;
	}
	public void ToggleDontDraw(bool flag)
	{
		if(flag)
			m_drawMode = EditorDrawMode.DontDraw;
	}
}
