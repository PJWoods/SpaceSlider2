using System;
using UnityEngine;

public class EditorState : GameState
{
	public override void Begin()
	{
		Game.Instance.LoadLevel("Editor");
		Game.Instance.UICore.Create<MapEditor>(Resources.Load("Prefabs/UI/EditorMenu") as GameObject);
	}

	public override void Update()
	{

	}

	public override void End()
	{
		Game.Instance.UICore.Clear();
	}
}

