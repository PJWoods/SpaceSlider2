using System;
using UnityEngine;

public class LaneChangerBlock : BlockBase
{
	private GameObject m_target;
	public int ChangeCount;

	void Start()
	{
		m_target =  GameObject.FindGameObjectWithTag("Player");

		if(!m_target)
			Debug.LogError("Couldnt find the object with tag Player");
		else if(m_target.GetComponent<PlayerMovement>() == null)
			Debug.LogError("The target need a PlayerMovement component for this script to take effect!");
	}
	protected override void Update () 
	{
	}
	public override void OnCollision ()
	{
		if(m_target != null)
			m_target.GetComponent<PlayerMovement>().ChangeLane(ChangeCount);
	}
}


