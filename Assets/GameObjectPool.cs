using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//////////[ExecuteInEditMode]
public class GameObjectPool : MonoBehaviour
{
    //A simple class that defines the prefab to hold in the pool and the amount of that gameobject
    [Serializable]
    public class PoolItem
    {
        public GameObject Prefab;
        public int PoolSize = 0;
    }
	//Show debug information (amount of pooled object etc.)
	public bool ShowDebugInfo = false;

	//A flag that controls the amount of pooled objects, 
	//if true, its possible to further add objects exceeding 
	//the pools inited size for each prefab in runtime 
	public bool AllowedToGrow = true;

	//List of prefabs to initiate the pool with
	public PoolItem[] Prefabs;

	//The actual list of all objects
	private List<GameObject>[] m_poolOfObjects;

	//The holding gameobject / the container
	private GameObject m_containerObject;

	void OnEnable ()
	{
        m_containerObject = new GameObject("GameObjectPool");
        m_poolOfObjects = new List<GameObject>[Prefabs.Length];

		//We build up the pools
        int prefabIndex = 0;
        foreach (PoolItem poolItem in Prefabs)
        {
            Debug.Assert(poolItem.Prefab != null, "You need to specify a prefab / gameobject to initate the pool with, check element: " + prefabIndex.ToString());
            Debug.Assert(poolItem.PoolSize > 0, "You tried to initiate a pool with size 0 with object type: " + poolItem.Prefab.name);
            m_poolOfObjects[prefabIndex] = new List<GameObject>();

            for (int n = 0; n < poolItem.PoolSize; ++n)
            {
                GameObject newObj = Instantiate(poolItem.Prefab) as GameObject;
                newObj.name = poolItem.Prefab.name;
                AddToPool(newObj);
            }
            ++prefabIndex;
        }
    }

	//I thought about not having the "instantiateIfEmpty" flag, but figured it gives me 
	//more control by giving me the option to instantiate a object or not when the pool is empty
	public GameObject GetFromPool(string objectType, bool instantiateIfEmpty)
	{
		for(int i = 0; i < Prefabs.Length; ++i)
		{
			PoolItem item = Prefabs[i];
			if(item.Prefab.name == objectType)
			{
				//Check so we have atleast 2 objects left, 1 is always kept as a backup for instantiating
				if(m_poolOfObjects[i].Count > 1)
				{
					//Take the first one(fast access) and remove it from the list 
					GameObject pooledObject = m_poolOfObjects[i][0];
					m_poolOfObjects[i].RemoveAt(0);
					pooledObject.transform.parent = null;
					pooledObject.SetActive(true);
					return pooledObject;
				} 

				//Instatitiates a new object if allowed
				if(instantiateIfEmpty) 
				{
					Debug.Log("Empty pool, consider increasing poolsize of object type: " + objectType);

					GameObject instantiatedObject = Instantiate(m_poolOfObjects[i][0]) as GameObject;
					instantiatedObject.transform.parent = null;
					instantiatedObject.name = m_poolOfObjects[i][0].name;
					instantiatedObject.SetActive(true);
					return instantiatedObject;
				}
				break;
			}
		}
		Debug.Assert(false, "The pool is empty and you didn't want to instantiate a new GameObject of type: " + objectType);
		return null;
	}

	public void AddToPool(GameObject gameObject)
	{
		for (int i = 0; i < Prefabs.Length; ++i)
		{
			if(Prefabs[i].Prefab.name == gameObject.name)
			{
				//We first check to see if the pools are allowed to grow
				if(AllowedToGrow)
				{
					gameObject.SetActive(false);
					gameObject.transform.parent = m_containerObject.transform;
					m_poolOfObjects[i].Add(gameObject);		
				}
				//else we check if the pool of the prefabtype already is full
				else if(Prefabs.Length < Prefabs[i].PoolSize)
				{
					gameObject.SetActive(false);
					gameObject.transform.parent = m_containerObject.transform;
					m_poolOfObjects[i].Add(gameObject);
				}
				else
				{
					Debug.Log("Tried to add the object type: " + gameObject.name + ", but the pool is already at maximum capacity.");
				}
				return;
			}
		}
		Debug.Assert(false, "You tried to add an object to the pool that isn't available! Object: " + gameObject.name);
	}

	void OnGUI()
	{
		if(ShowDebugInfo)
		{
			Rect screenRectangle = new Rect(5, 30, 400.0f, 200.0f);
			string text = "GameObject Pool:\n";

			int totalAvailableObjects = 0;
			int availablePerPrefabType = 0;
			for(int i = 0; i < m_poolOfObjects.Length; ++i)
			{
				if(m_poolOfObjects[i].Count > 0)
				{
					availablePerPrefabType = m_poolOfObjects[i].Count;
					totalAvailableObjects += availablePerPrefabType;
					text += m_poolOfObjects[i][0].name + ": " + availablePerPrefabType.ToString() + "\n";					
				}
				availablePerPrefabType = 0;
			}
			text += "Total Available Objects: " + totalAvailableObjects.ToString();
			GUI.Label(screenRectangle, text);
		}
	}
}
