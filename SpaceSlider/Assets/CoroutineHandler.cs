using UnityEngine;
using System.Collections;

public class CoroutineHandler : MonoBehaviour
{
    private static CoroutineHandler m_instance;

    private static CoroutineHandler Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject go = new GameObject();
                go.name = "CoroutineHandler";
                m_instance = go.AddComponent<CoroutineHandler>();
                DontDestroyOnLoad(go);
            }

            return m_instance;
        }
        set { m_instance = value; }
    }

    public static void Run (IEnumerator coroutine)
    {
        Instance.StartCoroutine(coroutine);
    }

    public void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != m_instance)
            {
                Destroy(gameObject);
            }
        }
    }

    
}
