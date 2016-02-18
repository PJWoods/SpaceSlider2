using UnityEngine;
using System.Collections;

public class InGameState : GameState
{
    public enum EntryAction
    {
        EditorEntry,
    }

    public class Context : ContextBase
    {
        public EntryAction EntryAction;
        public string Level;
    }

    public EntryAction Action { get { return m_entryAction; } }
    private EntryAction m_entryAction;

	public GameObject CurrentLevel { get { return m_currentLevel; } }
	private GameObject m_currentLevel;

	public GameObject Player { get { return m_player; } }
	private GameObject m_player;
//	public GameObject MainCamera { get { return m_mainCamera; } }
//	private GameObject m_mainCamera;

    public override void Begin(ContextBase context)
    {
        Context castedContext = context as Context;
		m_entryAction = castedContext.EntryAction;

        Game.Instance.LoadLevel("Game");

		if(castedContext != null)
		{
			m_currentLevel = GameObject.Instantiate(Resources.Load("Prefabs/Levels/EmptyLevel"), Vector3.zero, Quaternion.identity) as GameObject;
			Grid grid = m_currentLevel.GetComponent<Grid>();
			grid.InitAndLoadLevel(castedContext.Level);

			//m_mainCamera = GameObject.Instantiate(Resources.Load("Prefabs/MainCameraPrefab"), Vector3.zero, Quaternion.identity) as GameObject;
			GameObject cam = Camera.main.gameObject;
			cam.AddComponent<CameraMovement>();

			cam.GetComponent<CameraMovement>().Acceleration = 0.05f;
			cam.GetComponent<CameraMovement>().Velocity.y = 0.1f;

			m_player = GameObject.Instantiate(Resources.Load("Prefabs/PlayerPrefab"), Vector3.zero, Quaternion.identity) as GameObject;
			m_player.GetComponent<PlayerMovement>().SetCamera(cam);

//			m_mainCamera.GetComponent<CameraMovement>().SetTopSpeed(grid.GameSpeed);
//			m_player.GetComponent<PlayerMovement>().SetTopSpeed(grid.GameSpeed);
		}
    }

    public override void OnSceneDoneLoading()
    {

    }

    public override void Update()
    {

    }

    public override void End()
    {
        Game.Instance.UICore.Clear();
    }
}
