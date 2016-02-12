using UnityEngine;
using System.Collections;

public class LevelBase : MonoBehaviour {

	[ShowOnly] public string Name;
	[ShowOnly] public string Path;

	public Vector3 PlayerStart;
	public Vector3 CameraStart;

	[SerializeField]
	public GameObject PlayerObject;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void Init()
	{
		if(PlayerObject == null)
		{
			Debug.LogError("PlayerObject reference is null!");
		}
		Camera.main.transform.position = CameraStart;
		PlayerObject.transform.position = PlayerStart;		
	}
}
