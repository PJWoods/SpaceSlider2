public abstract class GameState
{
    public abstract void Begin();
    public abstract void Update();
    public abstract void End();
}

public class GameStateHandler
{
    private GameState m_currentState;

    public void Initialize()
    {
        SetState(new MenuState());
       
    }
    public void ChangeState(GameState state)
    {
        SetState(state);
    }

    private void SetState(GameState state)
    {
        if (m_currentState != null)
            m_currentState.End();

        m_currentState = state;

        m_currentState.Begin();
    }
}