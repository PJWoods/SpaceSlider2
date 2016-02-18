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

	private GameObject m_Camera;
	// Use this for initialization
	void Start () 
	{
		//MaximumDistance *= MaximumDistance;
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update () 
	{
		if(!m_isColliding)
		{
			CalculatedCameraDragAndMovement();
			if(m_changingLane)
			{
				Vector3 laneChangeDir = (m_laneChangeTarget - transform.position);
				if(laneChangeDir.sqrMagnitude > 0.01f && Mathf.Abs(Mathf.Abs(m_laneChangeTarget.x) - Mathf.Abs(transform.position.x)) > 0.01f)
				{
					float direction = (laneChangeDir.normalized.x > 0 ? 1f : -1f);
					m_currentVelocity.x = direction * m_currentVelocity.y * LaneChangeSpeed * Time.deltaTime;
				}
				else
				{
					Vector3 pos = transform.position;
					pos.x = m_laneChangeTarget.x;
					transform.position = pos;
					m_currentVelocity.x = 0;
					m_changingLane = false;
				}			
			}
			transform.position += m_currentVelocity;			
		}
	}

	void CalculatedCameraDragAndMovement()
	{
		Vector3 camPos = m_Camera.transform.position;
		camPos.x = transform.position.x;
		camPos.z = transform.position.z;

		float distance = Mathf.Abs(camPos.y - transform.position.y);
		if(distance < MaximumDistance)
		{
			Vector3 vel_diff = m_Camera.GetComponent<CameraMovement>().CurrentVelocity - m_currentVelocity;
			m_currentVelocity += vel_diff * Time.deltaTime;
			return;
		}

		float drag = distance / MaximumDistance;
		Vector3 direction = (camPos - transform.position).normalized;
		m_currentVelocity += (direction * Acceleration * Time.deltaTime) * drag * DragFactor;
	}

	public void ChangeLane(int count)
	{
		//GameObject level = GameState.CurrentLevel;
		//Grid grid = level.GetComponent<Grid>();
		/*Vector3 pos = transform.position;
		pos.y += grid.CellDimensions.y * 0.5f;

		Vector3 cellPos = grid.GetWorldPositionFromIndex(grid.GetCellIndexFromWorldPosition(pos));
		m_laneChangeTarget = cellPos + new Vector3(grid.CellDimensions.x * count, grid.CellDimensions.y * Mathf.Abs(count), cellPos.z);
		m_changingLane = true;*/
	}

	public void SetCamera(GameObject camera)
	{
		m_Camera = camera;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		m_isColliding = true;
	}
	void OnCollisionExit2D(Collision2D collision)
	{
		m_currentVelocity.y = 0f;
		m_isColliding = false;
	}
	void OnTriggerEnter2D(Collider2D collision)
	{
		m_isColliding = false;
		if(collision.gameObject.GetComponent<BlockBase>() != null)
		{
			collision.gameObject.GetComponent<BlockBase>().OnCollision();
		}
	}
}
