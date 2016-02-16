using UnityEngine;

public class GameState
{
    public class ContextBase
    {
        
    }

    public virtual void Begin(ContextBase context) {}
    public virtual void Update() {}
    public virtual void End() {}
}

public class GameStateHandler
{
	private GameState m_currentState;

    public void Initialize()
    {
        SetState(new MenuState());
       
    }
    public void ChangeState(GameState state, GameState.ContextBase context = null)
    {
        SetState(state, context);
    }
	public GameState GetCurrentState()
	{
		return m_currentState;
	}
    private void SetState(GameState state, GameState.ContextBase context = null)
    {
        if (m_currentState != null)
            m_currentState.End();

        m_currentState = state;

        if(context != null)
        {
            context = new GameState.ContextBase();
        }
        m_currentState.Begin(context);
    }
}