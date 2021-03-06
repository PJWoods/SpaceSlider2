﻿using UnityEngine;
using System.Collections;

public class Game
{
    private static Game m_instance;

    public UICore UICore;
    public GameStateHandler GameState;
	public EventManager EventManager;
	public GameObjectPool ObjectPool;
	public IOManager IOManager;

	public PlayerInfo PlayerInfo;

    public void GameStartUp()
    {
		ObjectPool = (GameObject.Instantiate(Resources.Load("Prefabs/System/GameObjectPool")) as GameObject).GetComponent<GameObjectPool>();
		IOManager = (GameObject.Instantiate(Resources.Load("Prefabs/System/IOManager")) as GameObject).GetComponent<IOManager>();

        UICore = (GameObject.Instantiate(Resources.Load("Prefabs/UI/UIRoot")) as GameObject).GetComponent<UICore>();
        GameState = new GameStateHandler();
        GameState.Initialize();

        PlayerInfo = new PlayerInfo();	
		EventManager = new EventManager();
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
	    GameState.OnSceneDoneLoading();
    }
}
