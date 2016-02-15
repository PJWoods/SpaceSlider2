using UnityEngine;

public abstract class GameState
{
	static public GameObject Player { get; protected set; }
	static public GameObject CurrentLevel;

	public abstract void Begin();
    public abstract void Update();
    public abstract void End();
}

public class GameStateHandler
{
	static public bool EditorMode = false;
	private GameState m_currentState;

    public void Initialize()
    {
        SetState(new MenuState());
       
    }
    public void ChangeState(GameState state)
    {
        SetState(state);
    }
	public GameState GetCurrentState()
	{
		return m_currentState;
	}
    private void SetState(GameState state)
    {
        if (m_currentState != null)
            m_currentState.End();

        m_currentState = state;

        m_currentState.Begin();
    }
}