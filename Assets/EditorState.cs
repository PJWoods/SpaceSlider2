using System;
using UnityEngine;

public class EditorState : GameState
{
	public override void Begin()
	{
		GameStateHandler.EditorMode = true;
		Game.Instance.LoadLevel("Editor");
		Game.Instance.UICore.Create<MapEditor>(Resources.Load("Prefabs/UI/EditorMenu") as GameObject);

		UnityEngine.Object[] baseArray = Resources.LoadAll("Prefabs/Blocks");
		Game.Instance.ObjectPool.Prefabs = new GameObjectPool.PoolItem[baseArray.Length];
		for(int i = 0; i < baseArray.Length; ++i)
		{
			GameObjectPool.PoolItem item = new GameObjectPool.PoolItem();
			item.Prefab = baseArray[i] as GameObject;
			item.PoolSize = 128;
			Game.Instance.ObjectPool.Prefabs[i] = item;
		}
		Game.Instance.ObjectPool.Init();

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

