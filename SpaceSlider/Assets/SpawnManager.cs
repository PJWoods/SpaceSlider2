using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

	public int SpawnDistance;
	private Vector3 m_lastCameraPosition;

	// Use this for initialization
	void Start () {
		m_lastCameraPosition = Camera.main.transform.position;
		SpawnDistance *= SpawnDistance;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float sqLenght = (Camera.main.transform.position - m_lastCameraPosition).sqrMagnitude;
		if(sqLenght > SpawnDistance)
		{
			m_lastCameraPosition = Camera.main.transform.position;
	
			Vector3 worldTopLeft = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
			Vector3 worldBottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));

			int blockCount = Random.Range(0, 3);
			for(int i = 0; i < blockCount; ++i)
			{
				string blockTypeId = "BlockType" + Random.Range(1, 3).ToString();
				GameObject block = GameObjectPool.Instance.GetFromPool(blockTypeId, false);

				worldBottomRight.y = worldTopLeft.y + ((worldTopLeft.y - worldBottomRight.y) / blockCount) * i;
				block.transform.position = worldBottomRight;				
			}
		}
	}
}
