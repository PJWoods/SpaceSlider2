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

		float windowHeight = Camera.main.orthographicSize * 2f;
		float windowWidth = (windowHeight / Screen.height * Screen.width);
		float targetAspect = 3f / 2f;
		float aspect = (windowHeight / windowWidth) / targetAspect;
		SlowAmount /= aspect;

		m_targets = new List<GameObject>(2);
		GameObject obj = GameObject.FindGameObjectWithTag("Player");

		if(!obj)
			Debug.LogError("Couldnt find the object with tag Player");
		m_targets.Add(obj);
		m_targets.Add(Camera.main.gameObject);

		m_lastTargetPositions = new List<Vector3>(m_targets.Count);
		foreach(GameObject g in m_targets)
			if(g != null)
				m_lastTargetPositions.Add(g.transform.position);

		if(FadeInTime > FadeOutTime || FadeInTime + FadeOutTime > Duration || FadeInTime < 0 || FadeOutTime < 0 || FadeOutTime > Duration)
			Debug.LogError("Timing error in SlowPowerUpBlock!");
	}

	protected override void Update()
	{
		base.Update();
		if(m_currentDuration < FadeInTime)
			m_currentSlowAmount += (SlowAmount / FadeInTime) * Time.deltaTime;
		else if(m_currentDuration > FadeOutTime && m_currentDuration < Duration)
			m_currentSlowAmount -= (SlowAmount / FadeOutTime) * Time.deltaTime;

		if(m_currentSlowAmount >= 0f)
		{	
			for(int i = 0; i < m_targets.Count; ++i)
			{
				if(m_targets[i] != null)
				{
					Vector3 relPosition = (m_targets[i].transform.position - m_lastTargetPositions[i]);
					Vector3 pos = (relPosition.normalized * m_currentSlowAmount) * Time.deltaTime;
					pos.x = 0f;
					m_targets[i].transform.position -= pos;							
				}			
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

