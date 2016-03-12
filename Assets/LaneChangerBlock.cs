using System;
using UnityEngine;

public class LaneChangerBlock : BlockBase
{
	[SerializeField] 
	private int ChangeCount;

	private GameObject m_target;

	private bool m_swiped;
	private float m_initialSelectionX;

	public void Start()
	{
		base.Start();
		m_target =  GameObject.FindGameObjectWithTag("Player");

		if(!m_target)
			Debug.Log("Couldnt find the object with tag Player");
		else if(m_target.GetComponent<PlayerMovement>() == null)
			Debug.Log("The target need a PlayerMovement component for this script to take effect!");
	}


	public override void OnCollision ()
	{
		if(m_target != null && BlockType != BlockProperty.LaneChangerSwipe)
			m_target.GetComponent<PlayerMovement>().ChangeLane(m_gridIndex, ChangeCount);
	}

	public override void UpdateMovement()
	{
		if(BlockType != BlockProperty.LaneChangerSwipe)
			return;

		if(m_target)
		{
			if(Input.GetMouseButtonUp(0))
			{
				m_swiped = false;
				if(m_parentCell.GetComponent<GridCell>().Inside(m_target.transform.position.x, m_target.transform.position.y))
				{
					if(Input.mousePosition.x < m_initialSelectionX - 32f)
						m_target.GetComponent<PlayerMovement>().ChangeLane(m_gridIndex, -1); //Left
					else if(Input.mousePosition.x > m_initialSelectionX + 32f)
						m_target.GetComponent<PlayerMovement>().ChangeLane(m_gridIndex, 1); //Right

					enabled = false;
				}			
			}
		}
		this.OnCollision();
	}

	protected override void Update () 
	{
		base.Update();
	}

	protected override void OnMouseDown()
	{
		base.OnMouseDown();

		if(BlockType != BlockProperty.LaneChangerSwipe)
			return;
		
		if(!m_swiped)
		{
			m_initialSelectionX = Input.mousePosition.x;
			m_swiped = true;
		}	
	}
}


