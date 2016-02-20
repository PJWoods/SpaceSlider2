using System;
using UnityEngine;

public class EditorState : GameState
{
	private string m_levelToLoad;
	private MapEditor m_editor;

	public override void Begin(ContextBase context)
	{
		m_entryAction = context.ActionOnEnter;
		m_levelToLoad = context.Level;

		Game.Instance.LoadLevel("Editor");
		m_editor = Game.Instance.UICore.Create<MapEditor>(Resources.Load("Prefabs/UI/EditorMenu") as GameObject);
	}
	public override void OnSceneDoneLoading()
	{
		if(m_entryAction == ContextBase.EntryAction.GameEntry)
		{
			string path = Application.dataPath + "/Levels/" + m_levelToLoad + ".lel";
			m_editor.Load(path);
		}
	}

	public override void Update()
	{

	}

	public override void End()
	{
		Game.Instance.UICore.Clear();
	}
}

