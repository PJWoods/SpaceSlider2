using System;
using UnityEngine;

public class PowerUpBlock : BlockBase
{
	public enum PowerUpType
	{
		Crush,
		Bomb,
		Slow
	};
	public float 			Duration;
	protected float			m_scaleMultiplier; 
	protected float			m_currentDuration;
	protected PowerUpType 	m_type;

	protected Vector3		m_scalingSteps;
	protected Vector3		m_targetScale;
	protected bool			m_overrideTriggerEffect;

	public virtual void Start()
	{
		base.Start();

		m_scaleMultiplier = -1;
		m_currentDuration = Duration;
	}

	protected new virtual void Update()
	{
		base.Update();
		if(!m_overrideTriggerEffect)
		{
			m_currentDuration += Time.deltaTime;
			transform.localScale += m_scalingSteps * Time.deltaTime * m_scaleMultiplier;
			if((m_targetScale - transform.localScale).sqrMagnitude < 0.01f)
			{
				m_parentCell.GetComponent<GridCell>().SetBlock(null);
				gameObject.SetActive(false);
			}			
		}
	}

	public override void OnCollision()
	{
		m_currentDuration = 0f;
		m_scalingSteps = transform.localScale / Duration;
		m_targetScale = transform.localScale + (Duration * m_scalingSteps * m_scaleMultiplier);
	}
}

