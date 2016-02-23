using UnityEngine;
using System.Collections;

public class BombPowerUp : PowerUpBlock 
{
	[SerializeField]
	private int 	ExplosionRadius;
	private bool	m_setToExplode;

	public override void Start()
	{
		base.Start();
		m_scaleMultiplier = 1;
		m_type = PowerUpType.Bomb;
	}

	protected override void Update()
	{
		if(m_setToExplode)
		{
			m_currentDuration += Time.deltaTime;
			transform.localScale += m_scalingSteps * Time.deltaTime * m_scaleMultiplier;
			if((m_targetScale - transform.localScale).sqrMagnitude < 0.01f || m_currentDuration > Duration)
			{
				for (int y = -ExplosionRadius; y <= ExplosionRadius; ++y) 
				{
					int currentY = (int)m_gridIndex.y + y;
					if(currentY < 0 || currentY >= m_grid.Cells.Y)
						continue;
					
					for (int x = -ExplosionRadius; x <= ExplosionRadius; ++x) 
					{
						if(x == 0 && y == 0)
							continue;

						int currentX = (int)m_gridIndex.x + x;
						if(currentX < 0 || currentX >= m_grid.Cells.X)
							continue;

						GameObject c = m_grid.GetCells()[currentY][currentX];
						GameObject b = c.GetComponent<GridCell>().GetBlock();
						if(b != null)
						{
							if( b.GetComponent<BlockBase>().BlockType == BlockProperty.NonMovable ||
								b.GetComponent<BlockBase>().BlockType == BlockProperty.Movable)
							{
								c.GetComponent<GridCell>().SetBlock(null);
								b.SetActive(false);
							}
							else if(b.GetComponent<BlockBase>().BlockType == BlockProperty.BombPowerUp)
							{
								// Triggers chain-reaction
								b.GetComponent<BombPowerUp>().OnCollision();
							}
						}
					}
				}
				m_parentCell.GetComponent<GridCell>().SetBlock(null);
				gameObject.SetActive(false);
			}
		}
	}
	public override void UpdateMovement()
	{
		if(m_setToExplode)
			return;
		
		this.OnCollision();
	}

	public override void OnCollision()
	{
		base.OnCollision();
		m_setToExplode = true;
	}
}