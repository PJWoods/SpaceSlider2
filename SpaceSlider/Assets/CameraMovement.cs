using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public bool ShowDebugInfo = false;
	[ShowOnly] public Vector3 CurrentVelocity  = new Vector3(0, 0, 0);
	public Vector3 Velocity = new Vector3(0, 0, 0);
	public float Acceleration = 0f;

	// Use this for initialization
	void Start () 
	{
	}

	public Vector3 GetCurrentVelocity()
	{
		return CurrentVelocity;
	}

	// Update is called once per frame
	void Update() 
    {
		if(!MapEditor.Instance)
		{
			float currentSpeed = CurrentVelocity.sqrMagnitude;
			float targetSpeed = Velocity.sqrMagnitude;
			if(targetSpeed - currentSpeed < 0.0001f)
			{
				CurrentVelocity = Velocity;
			}
			Vector3 target = Velocity - CurrentVelocity;
			CurrentVelocity += (target.normalized * Acceleration * Time.deltaTime);
			transform.position += CurrentVelocity;
		}
		else
		{
			UpdateFreeFly();			
		}
	}
	private void UpdateFreeFly()
	{
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if(scroll != 0f)
		{
			float currentOrto = Camera.main.orthographicSize;
			currentOrto += (scroll * -3);
			currentOrto = Mathf.Clamp(currentOrto, 1f, 15f);
			Camera.main.orthographicSize = currentOrto;
		}
		if(Input.GetMouseButton(1))
		{
			Vector3 centerPos = new Vector3();
			centerPos.x = Screen.width * 0.5f;
			centerPos.y = Screen.height * 0.5f;
			centerPos.z = Input.mousePosition.z;

			Vector3 diff = Input.mousePosition - centerPos;
			Vector3 direction = diff.normalized;
			float speed = diff.sqrMagnitude * 0.0001f;

			transform.position += direction * speed * Time.deltaTime;
		}
	}

	void OnGUI()
	{
		if(ShowDebugInfo)
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
			Rect screenRectangle = new Rect(0, Screen.height - 20, Screen.width, 20.0f);
			string text = "Camera position(" + transform.position.x.ToString() + ", " + transform.position.y.ToString() + ", " + transform.position.z.ToString() + ")"
				+ ", velocity(" + CurrentVelocity.x.ToString() + ", " + CurrentVelocity.y.ToString() + ", " + CurrentVelocity.z.ToString() + ")"
				+ ", screenMousePos(" + Input.mousePosition.x.ToString() + ", " + Input.mousePosition.y.ToString() + ")"
				+ ", worldMousePos(" + mousePos.x.ToString() + ", " + mousePos.y.ToString() + ")";
			GUI.Label(screenRectangle, text);

			if(Acceleration == 0)
				Debug.Log("The cameras acceleration is ZERO, resulting in no movement!");
		}
	}
}
