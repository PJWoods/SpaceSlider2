using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InGameState : GameState
{
	private string m_levelToLoad;

	public GameObject CurrentLevel { get { return m_currentLevel; } }
	private GameObject m_currentLevel;

	public GameObject Player { get { return m_player; } }
	private GameObject m_player;
//	public GameObject MainCamera { get { return m_mainCamera; } }
//	private GameObject m_mainCamera;

    public override void Begin(ContextBase context)
    {
		m_entryAction = context.ActionOnEnter;
		m_levelToLoad = context.Level;

        Game.Instance.LoadLevel("Game");
    }

    public override void OnSceneDoneLoading()
    {
		if(m_entryAction == ContextBase.EntryAction.EditorEntry)
		{
			m_currentLevel = GameObject.Instantiate(Resources.Load("Prefabs/Levels/EmptyLevel"), Vector3.zero, Quaternion.identity) as GameObject;
			Grid grid = m_currentLevel.GetComponent<Grid>();
			grid.InitAndLoadLevel(m_levelToLoad);

			//m_mainCamera = GameObject.Instantiate(Resources.Load("Prefabs/MainCameraPrefab"), Vector3.zero, Quaternion.identity) as GameObject;
			GameObject cam = Camera.main.gameObject;
			cam.AddComponent<CameraMovement>();
			cam.GetComponent<CameraMovement>().Acceleration = 0.01f;
			cam.GetComponent<CameraMovement>().Velocity.y = 0.05f;

			m_player = GameObject.Instantiate(Resources.Load("Prefabs/PlayerPrefab"), Vector3.zero, Quaternion.identity) as GameObject;
			m_player.GetComponent<PlayerMovement>().SetCamera(cam);
			m_player.GetComponent<PlayerMovement>().SetGrid(grid);

			//m_mainCamera.GetComponent<CameraMovement>().SetTopSpeed(grid.GameSpeed);
			//m_player.GetComponent<PlayerMovement>().SetTopSpeed(grid.GameSpeed);

			GameObject callbackObject = GameObject.Instantiate(Resources.Load("Prefabs/System/ReturnToEditorPrefab"), Vector3.zero, Quaternion.identity) as GameObject;
			callbackObject.GetComponent<ReturnToEditorScript>().SetCallback(ReturnToEditor);
		}
    }

    public override void Update()
    {
    }

    public override void End()
    {
        Game.Instance.UICore.Clear();
    }

	private void ReturnToEditor()
	{
		ContextBase context = new ContextBase();
		context.ActionOnEnter = ContextBase.EntryAction.GameEntry;
		context.Level = m_levelToLoad;
		Game.Instance.GameState.ChangeState(new EditorState(), context);
	}
}
