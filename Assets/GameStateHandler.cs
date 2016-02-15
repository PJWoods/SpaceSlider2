using System.Collections.Generic;
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
	private List<GameState> m_states;

    public void Initialize()
    {
        SetState(new MenuState());
    }
    public void ChangeState(GameState state)
    {
        SetState(state);
    }
	public void Push(GameState state)
	{
		state.Begin();
		m_states.Insert(0, state);
		m_currentState = state;
	}
	public void Pop()
	{
		if(m_states.Count > 0)
		{
			m_states[0].End();
			m_states.RemoveAt(0);

			if(m_states.Count > 0)
				m_currentState = m_states[0];
			else
				m_currentState = null;
		}
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