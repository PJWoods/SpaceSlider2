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
				Vector3 currentScreenPos = Camera.main.WorldToScreenPoint(transform.position);

				GameObject grid = GameObject.Find("Grid");
				Grid gridComponent = grid.GetComponent<Grid>();
				GridCell cellCheck = gridComponent.GetCellFromScreenPosition(new Vector2(currentScreenPos.x, Input.mousePosition.y));
				if(cellCheck != null && cellCheck != m_parentCell)
					if(cellCheck.GetBlock() != null)
						return;

				currentScreenPos.y = Mathf.Lerp(currentScreenPos.y, Input.mousePosition.y, Time.deltaTime * 25f);

				Vector3 currentWorldPos = Camera.main.ScreenToWorldPoint(currentScreenPos);
				transform.position = currentWorldPos;

				Vector3 direction = (transform.position - m_parentCell.GetPosition());
				float currentLenght = direction.sqrMagnitude;
				currentLenght *= currentLenght;

				direction.Normalize();
				float limit = (m_parentCell.GetDimensions().y * 0.2f) * (m_parentCell.GetDimensions().y * 0.2f);
				if(currentLenght >= limit)
				{
					Vector3 worldPos = m_parentCell.GetPosition();
					worldPos += direction * m_parentCell.GetDimensions().y;

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

