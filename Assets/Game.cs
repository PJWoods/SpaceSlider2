using UnityEngine;
using System.Collections;

public class Game
{
    private static Game m_instance;

    public UICore UICore;
    public GameStateHandler GameState;
    
	public GameObjectPool ObjectPool;
	public IOManager IOManager;

	public PlayerInfo PlayerInfo;

    public void GameStartUp()
    {
		ObjectPool = new GameObjectPool();

		Object[] baseArray = Resources.LoadAll("Prefabs/Blocks");
		ObjectPool.Prefabs = new GameObjectPool.PoolItem[baseArray.Length];
		for(int i = 0; i < baseArray.Length; ++i)
		{
			GameObjectPool.PoolItem item = new GameObjectPool.PoolItem();
			item.Prefab = baseArray[i] as GameObject;
			item.PoolSize = 128;
			ObjectPool.Prefabs[i] = item;
		}
		IOManager = new IOManager();

        UICore = (GameObject.Instantiate(Resources.Load("Prefabs/UI/UIRoot")) as GameObject).GetComponent<UICore>();
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
