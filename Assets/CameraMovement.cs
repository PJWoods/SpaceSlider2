using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public bool ShowDebugInfo = false;
	public Vector3 CurrentVelocity  = new Vector3(0, 0, 0);
	public Vector3 Velocity = new Vector3(0, 0, 0);
	public float Acceleration = 0f;

	// Use this for initialization
	void Start () 
	{
		float windowHeight = Camera.main.orthographicSize * 2f;
		float windowWidth = (windowHeight / Screen.height * Screen.width);

		float targetAspect = 3f / 2f;
		float aspect = (windowHeight / windowWidth) / targetAspect;
		Velocity /= aspect;
	}

	public Vector3 GetCurrentVelocity()
	{
		return CurrentVelocity;
	}

	// Update is called once per frame
	void Update() 
    {
		Vector3 target = Velocity - CurrentVelocity;
		CurrentVelocity += (target.normalized * Acceleration * Time.deltaTime);

		float currentSpeed = CurrentVelocity.sqrMagnitude;
		float targetSpeed = Velocity.sqrMagnitude;
		if(targetSpeed - currentSpeed < 0.0001f || currentSpeed > targetSpeed)
		{
			CurrentVelocity = Velocity;
		}
		transform.position += CurrentVelocity;
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
