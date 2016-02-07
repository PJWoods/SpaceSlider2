using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float Acceleration;
	public float LaneChangeSpeed;
	public float MaximumDistance;
	public float DragFactor;

	private Vector3 m_currentVelocity;
	private bool m_isColliding = false;

	private Vector3 m_laneChangeTarget;
	private bool m_changingLane = false;

	// Use this for initialization
	void Start () 
	{
		//MaximumDistance *= MaximumDistance;
	}

	// Update is called once per frame
	void Update () 
	{
		if(!MapEditor.Instance)
		{
			if(!m_isColliding)
			{
				CalculatedCameraDragAndMovement();
				if(m_changingLane)
				{
					Vector3 laneChangeDir = (m_laneChangeTarget - transform.position);
					if(laneChangeDir.sqrMagnitude > 0.01f && Mathf.Abs(Mathf.Abs(m_laneChangeTarget.y) - Mathf.Abs(transform.position.y)) > 0.01f)
					{
						float direction = (laneChangeDir.normalized.y > 0 ? 1f : -1f);
						m_currentVelocity.y = direction * m_currentVelocity.x * LaneChangeSpeed * Time.deltaTime;
					}
					else
					{
						Vector3 pos = transform.position;
						pos.y = m_laneChangeTarget.y;
						transform.position = pos;
						m_currentVelocity.y = 0;
						m_changingLane = false;
					}			
				}
				transform.position += m_currentVelocity;			
			}
		}
	}

	void CalculatedCameraDragAndMovement()
	{
		Vector3 camPos = Camera.main.transform.position;
		camPos.y = transform.position.y;
		camPos.z = transform.position.z;

		float distance = Mathf.Abs(camPos.x - transform.position.x);
		if(distance < MaximumDistance)
		{
			Vector3 vel_diff = Camera.main.GetComponent<CameraMovement>().GetCurrentVelocity() - m_currentVelocity;
			m_currentVelocity += vel_diff * Time.deltaTime;
			return;
		}

		float drag = distance / MaximumDistance;
		Vector3 direction = (camPos - transform.position).normalized;
		m_currentVelocity += (direction * Acceleration * Time.deltaTime) * drag;
	}

	public void ChangeLane(int count)
	{
		GameObject gridObject = GameObject.Find("Grid");
		Grid grid = gridObject.GetComponent<Grid>();
		Vector3 pos = transform.position;
		pos.x += grid.CellDimensions.x * 0.5f;

		GridCell currentCell = grid.GetCellFromWorldPosition(pos);
		Vector3 cellPos = currentCell.GetPosition();
		m_laneChangeTarget = cellPos + new Vector3(grid.CellDimensions.x * Mathf.Abs(count), grid.CellDimensions.y * count);
		m_changingLane = true;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		m_isColliding = true;
	}
	void OnCollisionExit2D(Collision2D collision)
	{
		m_currentVelocity.x = 0f;
		m_isColliding = false;
	}
	void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.GetComponent<BlockBase>() != null)
		{
			collision.gameObject.GetComponent<BlockBase>().OnCollision();
		}
	}
//	void OnTriggerStay2D(Collider2D collision)
//	{
//		if(collision.gameObject.GetComponent<BlockBase>().BlockType == BlockBase.BlockProperty.PowerUp)
//		{
//
//		}
//	}
//	void OnTriggerExit2D(Collider2D collision)
//	{
//		if(collision.gameObject.GetComponent<BlockBase>().BlockType == BlockBase.BlockProperty.PowerUp)
//		{
//			GameObjectPool.Instance.AddToPool(collision.gameObject);
//		}
//	}
}
