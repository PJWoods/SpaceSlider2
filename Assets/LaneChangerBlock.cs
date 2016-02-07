using System;
using UnityEngine;

public class LaneChangerBlock : BlockBase
{
	public GameObject Target;
	public int ChangeCount;

	void Start()
	{
		if(Target.GetComponent<PlayerMovement>() == null)
			Debug.LogError("The target need a PlayerMovement component for this script to take effect!");
	}
	protected override void Update () 
	{
	}
	public override void OnCollision ()
	{
		if(Target.GetComponent<PlayerMovement>() != null)
		{
			Target.GetComponent<PlayerMovement>().ChangeLane(ChangeCount);
		}
	}
}


