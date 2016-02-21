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
			GameObject.DestroyImmediate(Camera.main.gameObject);
			GameObject cam = GameObject.Instantiate(Resources.Load("Prefabs/MainCameraPrefab"), Vector3.zero, Quaternion.identity) as GameObject;

			m_currentLevel = GameObject.Instantiate(Resources.Load("Prefabs/Levels/EmptyLevel"), Vector3.zero, Quaternion.identity) as GameObject;
			Grid grid = m_currentLevel.GetComponent<Grid>();
			grid.InitAndLoadLevel(m_levelToLoad);

			m_player = GameObject.Instantiate(Resources.Load("Prefabs/PlayerPrefab")) as GameObject;
			m_player.GetComponent<PlayerMovement>().SetCamera(cam);
			m_player.GetComponent<PlayerMovement>().SetGrid(grid);

			Sprite s = m_player.GetComponent<SpriteRenderer>().sprite;
			float windowHeight = Camera.main.orthographicSize * 2f;
			float windowWidth = windowHeight / Screen.height * Screen.width;
			float spriteHeight = s.bounds.size.y;
			float spriteWidth = s.bounds.size.x;
			float scale = (windowWidth / spriteWidth) * 0.05f;
			m_player.transform.localScale = new Vector2(scale, scale);

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
