using System;
using UnityEngine;

public class MovableBlock : BlockBase
{
	public float SlidingSpeed;

	protected override void Update()
	{
		if(m_parentCell != null && !MapEditor.Instance)
		{
			base.Update();
			Vector3 parentPos = m_parentCell.GetPosition();
			Vector3 direction = (parentPos - transform.position);
			float length = direction.sqrMagnitude;
			direction.Normalize();

			if(length < 0.05f)
				transform.position = parentPos;
			else
				transform.position += direction * SlidingSpeed * Time.deltaTime;
		}
	}

	public override void UpdateMovement()
	{
		if(m_parentCell != null)
		{
			if(BlockType == BlockProperty.Movable)
			{
				GameObject grid = GameObject.Find("Grid");
				Grid gridComponent = grid.GetComponent<Grid>();
		
				Vector3 currentScreenPos = Camera.main.WorldToScreenPoint(transform.position);
				currentScreenPos.x = Mathf.Lerp(currentScreenPos.x, Input.mousePosition.x, Time.deltaTime * 25f);

				Vector3 currentWorldPos = Camera.main.ScreenToWorldPoint(currentScreenPos);
				transform.position = currentWorldPos;

				if(Mathf.Abs(transform.position.x - m_parentCell.GetPosition().x) > 0.001f)
				{
					float nextdoorCellX = -gridComponent.CellDimensions.x;
					if(transform.position.x > m_parentCell.GetPosition().x)
						nextdoorCellX = gridComponent.CellDimensions.x;

					GridCell cellCheck = gridComponent.GetCellFromWorldPosition(new Vector3(m_parentCell.GetPosition().x + nextdoorCellX, m_parentCell.GetPosition().y, m_parentCell.GetPosition().z));
					if(cellCheck != null && cellCheck != m_parentCell)
					{
						if(cellCheck.GetBlock() != null)
						{
							//Return here because the target cell has block
							transform.position = m_parentCell.GetPosition();
							return;
						}
					}
					else
					{
						transform.position = m_parentCell.GetPosition();
						return;
					}
				}

				Vector3 direction = (transform.position - m_parentCell.GetPosition());
				float currentLenght = direction.sqrMagnitude;
				currentLenght *= currentLenght;

				direction.Normalize();
				float limit = (m_parentCell.GetDimensions().x * 0.2f) * (m_parentCell.GetDimensions().x * 0.2f);
				if(currentLenght >= limit)
				{
					Vector3 worldPos = m_parentCell.GetPosition();
					worldPos += direction * m_parentCell.GetDimensions().x;
					GridCell cell = gridComponent.GetCellFromWorldPosition(worldPos);
					if(cell != null)
					{
						if(cell.GetBlock() == null)
						{
							m_parentCell.SetBlock(null);
							cell.SetBlock(this);						
						}
					}
				}
			}
		}
	}
	public override void OnCollision()
	{
	}
}

