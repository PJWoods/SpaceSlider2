using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UICore : MonoBehaviour
{
    List<GameObject> m_loadedElements;
    private void Awake()
    {
        m_loadedElements = new List<GameObject>();
        DontDestroyOnLoad(gameObject);
    }
	public T Create<T>(GameObject prefab)
    {
        GameObject element = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        m_loadedElements.Add(element);

        element.transform.SetParent(transform,false);
        return element.GetComponent<T>();
    }
    public void Clear()
    {
        for (int i = 0; i < m_loadedElements.Count; i++)
        {
            if(m_loadedElements[i] != null)
                Destroy(m_loadedElements[i]);
        }
        m_loadedElements.Clear();
    }
}
