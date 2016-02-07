using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlowPowerUpBlock : PowerUpBlock
{
	public List<GameObject>	Targets;
	public float			FadeInTime;
	public float			FadeOutTime;
	public float 			SlowAmount;

	private List<Vector3>	m_lastTargetPositions;
	private float			m_currentSlowAmount;

	public override void Start()
	{
		base.Start();
		m_type = PowerUpType.Slow;
		m_lastTargetPositions = new List<Vector3>(Targets.Count);
		foreach(GameObject g in Targets)
			m_lastTargetPositions.Add(g.transform.position);
		if(FadeInTime > FadeOutTime || FadeInTime + FadeOutTime > Duration || FadeInTime < 0 || FadeOutTime < 0 || FadeOutTime > Duration)
			Debug.LogError("Timing error in SlowPowerUpBlock!");
	}

	protected override void Update()
	{
		if(!MapEditor.Instance)
		{
			for(int i = 0; i < Targets.Count; ++i)
			{
				if(Targets[i] != null)
				{
					base.Update();

					if(m_currentDuration < FadeInTime)
						m_currentSlowAmount += (SlowAmount / FadeInTime) * Time.deltaTime;
					else if(m_currentDuration < FadeOutTime)
						m_currentSlowAmount -= (SlowAmount / FadeOutTime) * Time.deltaTime;

					Targets[i].transform.position -= ((Targets[i].transform.position - m_lastTargetPositions[i]).normalized * m_currentSlowAmount) * Time.deltaTime;
				}			
			}
		}
	}

	public override void OnCollision()
	{
		base.OnCollision();
		for(int i = 0; i < Targets.Count; ++i)
		{
			if(Targets[i] != null)
			{
				m_lastTargetPositions[i] = Targets[i].transform.position;
				m_currentSlowAmount = 0;
			}
		}
	}
}

