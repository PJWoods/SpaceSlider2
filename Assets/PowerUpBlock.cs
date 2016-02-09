using System;
using UnityEngine;

public class PowerUpBlock : MovableBlock
{
	public enum PowerUpType
	{
		Crush,
		Bomb,
		Slow
	};
	public float 			Duration;
	protected float			m_currentDuration;
	protected PowerUpType 	m_type;
	private Vector3			m_scalingSteps;

	public virtual void Start()
	{
		m_currentDuration = Duration;
	}

	protected override void Update()
	{
		if(!MapEditor.Instance)
		{
			m_currentDuration += Time.deltaTime;
			transform.localScale -= m_scalingSteps * Time.deltaTime;
			if(transform.localScale.sqrMagnitude < 0.001f)
				Game.Instance.ObjectPool.AddToPool(this.gameObject);
		}			
	}

	public override void OnCollision()
	{
		m_currentDuration = 0f;
		m_scalingSteps = transform.localScale / Duration;
	}
}

