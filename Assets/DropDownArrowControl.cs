using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownArrowControl : MonoBehaviour {

	public Dropdown Parent;
	private int m_currentSelectionIndex;
	void OnEnable()
	{
		m_currentSelectionIndex = 0;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			--m_currentSelectionIndex;
			if(m_currentSelectionIndex < 0)
				m_currentSelectionIndex = Parent.options.Count - 1;

			Parent.value = m_currentSelectionIndex;
			Parent.Select();
		}
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			++m_currentSelectionIndex;
			if(m_currentSelectionIndex >= Parent.options.Count)
				m_currentSelectionIndex = 0;

			Parent.value = m_currentSelectionIndex;
			Parent.Select();
		}
	}
}
