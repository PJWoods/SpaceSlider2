using UnityEngine;
using System.Collections;

public class InGameState : GameState
{
    public override void Begin()
    {
        Game.Instance.LoadLevel("assets");

		if(Camera.main.gameObject.GetComponent<CameraMovement>() == null)
			Camera.main.gameObject.AddComponent<CameraMovement>();

		if(Player == null)
			Player = GameObject.Instantiate(Resources.Load("Prefabs/Levels/PlayerPrefab"), Vector3.zero, Quaternion.identity) as GameObject;
    }

    public override void Update()
    {

    }

    public override void End()
    {
        Game.Instance.UICore.Clear();
    }
}
