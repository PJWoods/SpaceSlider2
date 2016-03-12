using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	public float Acceleration;
	public float LaneChangeSpeed;
	public float MaximumDistance;
	public float DragFactor;

	private Vector3 m_currentVelocity;
	private bool m_isColliding = false;

	private Vector3 m_laneChangeTarget;
	private float m_targetLaneChangeDistant;
	private float m_currentLaneChangeDistant;
	private bool m_changingLane = false;

	private Vector3 m_velocityBeforeLaneChange;

	public Grid GridRef { set { m_grid = GridRef; } get { return m_grid; } }
	private Grid m_grid;

	private GameObject m_camera;
	// Use this for initialization
	void Start () 
	{
		//MaximumDistance *= MaximumDistance;
	}

	// Update is called once per frame
	void Update () 
	{
		if(!m_isColliding)
		{
			if(m_changingLane)
			{
				if(m_currentLaneChangeDistant <= m_targetLaneChangeDistant - 0.01f)
				{
					m_currentLaneChangeDistant += Mathf.Abs(m_currentVelocity.x);
				}
				else
				{
					m_currentLaneChangeDistant = 0f;
					m_currentVelocity = m_velocityBeforeLaneChange;
					m_currentVelocity.x = 0;
					m_changingLane = false;
				}			
			}
			else
				CalculatedCameraDragAndMovement();
			transform.position += m_currentVelocity;			
		}
	}

	void CalculatedCameraDragAndMovement()
	{
		if(!m_changingLane)
			m_currentVelocity.x = 0f;

		Vector3 camPos = m_camera.transform.position;
		float distance = camPos.y - transform.position.y;
		if(Mathf.Abs(distance) < MaximumDistance)
		{
			//float vel_diff = m_camera.GetComponent<CameraMovement>().CurrentVelocity.y - m_currentVelocity.y;
			m_currentVelocity.y += Time.deltaTime * (distance - MaximumDistance) * Acceleration;
			return;
		}
		camPos.x = transform.position.x;
		camPos.z = transform.position.z;

		float drag = distance / MaximumDistance;
		Vector3 direction = (camPos - transform.position).normalized;
		direction.x = 0f;
		m_currentVelocity += (direction * Acceleration * Time.deltaTime) * drag * DragFactor;
	}

	public void ChangeLane(Vector2 originIndex, int count)
	{
		if(!m_changingLane)
			m_velocityBeforeLaneChange = m_currentVelocity;
		
		Vector2 currentCellIndex = originIndex;
		currentCellIndex.x += count; 

		m_laneChangeTarget = m_grid.GetWorldPositionFromIndex(currentCellIndex);
		m_laneChangeTarget.z = 0.3f;

		m_currentLaneChangeDistant = 0f;
		m_targetLaneChangeDistant = Mathf.Abs(m_laneChangeTarget.x - transform.position.x);

		Vector3 direction = (m_laneChangeTarget - transform.position).normalized;
		m_currentVelocity = (direction * LaneChangeSpeed) * m_grid.CellDimensions.x;
		m_currentVelocity.y = (m_velocityBeforeLaneChange.y + Mathf.Abs(m_currentVelocity.x)) * 0.5f;
		m_changingLane = true;
	}

	public void SetCamera(GameObject camera)
	{
		m_camera = camera;
	}
	public void SetGrid(Grid grid)
	{
		m_grid = grid;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		m_isColliding = true;
		InGameState state = Game.Instance.GameState.GetCurrentState() as InGameState;
		state.Restart();
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
