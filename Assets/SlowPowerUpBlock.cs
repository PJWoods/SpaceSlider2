using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlowPowerUpBlock : PowerUpBlock
{
	private List<GameObject>	m_targets;
	public float				FadeInTime;
	public float				FadeOutTime;
	public float 				SlowAmount;

	private List<Vector3>		m_lastTargetPositions;
	private float				m_currentSlowAmount;

	public override void Start()
	{
		base.Start();
		m_type = PowerUpType.Slow;

		m_targets = new List<GameObject>(2);
		/*GameObject obj = GameState.Player;

		if(!obj)
			Debug.LogError("Couldnt find the object with tag Player");
		m_targets.Add(obj);
		m_targets.Add(Camera.main.gameObject);

		m_lastTargetPositions = new List<Vector3>(m_targets.Count);
		foreach(GameObject g in m_targets)
			if(g != null)
				m_lastTargetPositions.Add(g.transform.position);

		if(FadeInTime > FadeOutTime || FadeInTime + FadeOutTime > Duration || FadeInTime < 0 || FadeOutTime < 0 || FadeOutTime > Duration)
			Debug.LogError("Timing error in SlowPowerUpBlock!");*/
	}

	protected override void Update()
	{
		for(int i = 0; i < m_targets.Count; ++i)
		{
			if(m_targets[i] != null)
			{
				base.Update();

				if(m_currentDuration < FadeInTime)
					m_currentSlowAmount += (SlowAmount / FadeInTime) * Time.deltaTime;
				else if(m_currentDuration < FadeOutTime)
					m_currentSlowAmount -= (SlowAmount / FadeOutTime) * Time.deltaTime;

				m_targets[i].transform.position -= ((m_targets[i].transform.position - m_lastTargetPositions[i]).normalized * m_currentSlowAmount) * Time.deltaTime;
			}			
		}
	}

	public override void OnCollision()
	{
		base.OnCollision();
		for(int i = 0; i < m_targets.Count; ++i)
		{
			if(m_targets[i] != null)
			{
				m_lastTargetPositions[i] = m_targets[i].transform.position;
				m_currentSlowAmount = 0;
			}
		}
	}
}

