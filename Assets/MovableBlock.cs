using System;
using UnityEngine;

public class MovableBlock : BlockBase
{
	public float SlidingSpeed;
	protected override void Update()
	{
		if(!MapEditor.Instance)
		{
			base.Update();
			Vector3 parentPos = m_grid.GetWorldPositionFromIndex(m_gridIndex);
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
		if(BlockType == BlockProperty.Movable)
		{
			Grid gridComponent = m_grid;
	
			Vector3 currentScreenPos = Camera.main.WorldToScreenPoint(transform.position);
			currentScreenPos.x = Mathf.Lerp(currentScreenPos.x, Input.mousePosition.x, Time.deltaTime * 25f);

			Vector3 currentWorldPos = Camera.main.ScreenToWorldPoint(currentScreenPos);
			transform.position = currentWorldPos;

			Vector3 parentPos = m_grid.GetWorldPositionFromIndex(m_gridIndex);
			if(Mathf.Abs(transform.position.x - parentPos.x) > 0.001f)
			{
				float nextdoorCellX = -gridComponent.CellDimensions.x;
				if(transform.position.x > parentPos.x)
					nextdoorCellX = gridComponent.CellDimensions.x;

				GameObject cellCheck = gridComponent.GetCellFromWorldPosition(new Vector3(parentPos.x + nextdoorCellX, parentPos.y, parentPos.z));
				if(cellCheck != null && cellCheck != this)
				{
					//Return here because the target cell has block
					transform.position = parentPos;
					return;
				}
				else
				{
					if((m_gridIndex.x <= 0 && parentPos.x + nextdoorCellX < parentPos.x) ||
						(m_gridIndex.x >= m_grid.Cells.X - 1 && parentPos.x + nextdoorCellX > parentPos.x))
					{
						transform.position = parentPos;
						return;					
					}
				}
			}

			Vector3 direction = (transform.position - parentPos);
			float currentLenght = direction.sqrMagnitude;
			currentLenght *= currentLenght;

			direction.Normalize();
			float limit = (m_grid.CellDimensions.x * 0.5f) * (m_grid.CellDimensions.x * 0.5f);
			if(currentLenght >= limit)
			{
				Vector3 worldPos = parentPos;
				worldPos += direction * m_grid.CellDimensions.x;
				Vector3 currentWorld = transform.position;
				m_grid.AddCellAtWorldPosition(null, parentPos);
				m_grid.AddCellAtWorldPosition(gameObject, worldPos);
				gameObject.transform.position = currentWorld;
				m_grid.SetSelectedBlock(null);
			}
		}
	}
	public override void OnCollision()
	{
	}
}

