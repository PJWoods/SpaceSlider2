using UnityEngine;

public class GameState
{
    public class ContextBase
    {
		public enum EntryAction
		{
			None,
			EditorEntry,
			GameEntry,
		}
		public EntryAction ActionOnEnter = EntryAction.None;
		public string Level;
	}

	public ContextBase.EntryAction Action { get { return m_entryAction; } }
	protected ContextBase.EntryAction m_entryAction;

    public virtual void Begin(ContextBase context) {}
    public virtual void Update() {}
    public virtual void End() {}
    public virtual void OnSceneDoneLoading() {}
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
    public void OnSceneDoneLoading()
    {
        m_currentState.OnSceneDoneLoading();
    }
    private void SetState(GameState state, GameState.ContextBase context = null)
    {
        if (m_currentState != null)
            m_currentState.End();

        m_currentState = state;

        if(context == null)
        {
            context = new GameState.ContextBase();
        }
        m_currentState.Begin(context);
    }
}