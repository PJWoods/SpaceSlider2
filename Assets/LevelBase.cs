using UnityEngine;
using System.Collections;

public class LevelBase : MonoBehaviour {

	[ShowOnly] public string Name;
	[ShowOnly] public string Path;

	public Vector3 PlayerStart;
	public Vector3 CameraStart;

	[SerializeField]
	private GameObject PlayerObject;

	// Use this for initialization
	void Start () 
	{
		Camera.main.transform.position = CameraStart;
		PlayerObject.transform.position = PlayerStart;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
