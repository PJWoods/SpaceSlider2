using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class IOManager : MonoBehaviour 
{
    //Bara för att det ska vara lätt,for now.
    public class MapRow
    {
        public List<int> Tiles = new List<int>();
    }
    public class Map
    {
        public List<MapRow> Rows = new List<MapRow>();
    }

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	
}
