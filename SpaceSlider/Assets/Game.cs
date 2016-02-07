using UnityEngine;
using System.Collections;

public class Game
{
    private static Game m_instance;

    public UICore UICore;
    public GameStateHandler GameState;
    public PlayerInfo PlayerInfo;

    public void GameStartUp()
    {
        UICore = (GameObject.Instantiate(Resources.Load("Prefabs/GUI/UIRoot")) as GameObject).GetComponent<UICore>();
        GameState = new GameStateHandler();
        GameState.Initialize();

        PlayerInfo = new PlayerInfo();	
    }

    public static Game Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new Game();
            }

            return m_instance;
        }
        set { m_instance = value; }
    }

    public void LoadLevel(string level)
    {
        CoroutineHandler.Run(LoadLevelAsync(level));
    }

    private IEnumerator LoadLevelAsync(string level)
    {
        Application.LoadLevelAsync(level);

        while (Application.isLoadingLevel)
            yield return null;

        OnSceneDoneLoading();
    }
    private void OnSceneDoneLoading()
    {
        
    }
}
