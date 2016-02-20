using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class MapEditor : MonoBehaviour
{
    public const int DEFAULT_ROWS = 64;
    public const int DEFAULT_ROW_WIDTH = 5;
    public const int DEFAULT_SPEED = 100;

    public enum EditorDrawMode
    {
        DrawEmpty,
        DrawMovable,
        DrawNonMovable,
        DrawBoost,
        DrawLaneChangerLeft,
        DrawLaneChangerRight
    }
    public class MapRow
    {
        public List<EditorTileButton> Tiles = new List<EditorTileButton>(DEFAULT_ROW_WIDTH);
    }
    public class Map
    {
        public List<MapRow> Rows = new List<MapRow>(DEFAULT_ROWS);

        public void Destroy()
        {
            for (int y = 0; y < Rows.Count; y++)
            {
                for (int x = 0; x < Rows[y].Tiles.Count; x++)
                {
                    GameObject.Destroy(Rows[y].Tiles[x].gameObject);
                }
            }
        }
    }

    [SerializeField]
    private Sprite[] m_tileTextures;

    [SerializeField]
    private GameObject m_tilePrefab;

    [SerializeField]
    private GameObject m_tileContainer;

    [SerializeField]
    private int m_mapWith;

    [SerializeField]
    private int m_mapHeight;

    [SerializeField]
    private Dropdown m_tileDropDown;

    [SerializeField]
    private Button m_saveButton;

    [SerializeField]
    private Button m_loadButton;

    [SerializeField]
    private Button m_clearButton;

    [SerializeField]
    private Button m_playButton;

    [SerializeField]
    private InputField m_speedLabel;

    [SerializeField]
    private InputField m_rowLabel;

    [SerializeField]
    private RectTransform scrollViewContentTransform;

    //Map specific values
    private Map m_map;
    private int m_speed = DEFAULT_SPEED;
    private int m_rows = DEFAULT_ROWS;
    private int m_rowWidth = DEFAULT_ROW_WIDTH;
    private string m_level = "default";

    private EditorTileButton m_lastClickedTile;
    
    private void Start()
    {
        //Add event methods to relavant callbacks.
        m_saveButton.onClick.AddListener(delegate { SaveButtonClicked(); });
        m_loadButton.onClick.AddListener(delegate { LoadButtonClicked(); });
        m_clearButton.onClick.AddListener(delegate { ClearButtonClicked(); });
        m_playButton.onClick.AddListener(delegate { PlayButtonClicked(); });
        m_rowLabel.onValueChanged.AddListener(delegate { RowValueChanged(); });
        m_speedLabel.onValueChanged.AddListener(delegate { SpeedValueChanged(); });

        InitMap(m_rows, m_rowWidth);
    }

    //Init map and hook up buttons.
    private void InitMap(int height, int width, StreamReader reader = null)
    {
        //Wipe the old one first
        if(m_map != null)
        {
            m_map.Destroy();
        }

        m_map = new Map();
        for (int y = 0; y < height; y++)
        {
            m_map.Rows.Add(new MapRow());

            MapRow currentRow = m_map.Rows[y];
            for (int x = 0; x < width; x++)
            {
                EditorTileButton tile = GameObject.Instantiate(m_tilePrefab).GetComponent<EditorTileButton>();
                currentRow.Tiles.Add(tile);

                SetTilePosition(tile, x, y);
                tile.transform.parent = m_tileContainer.transform;

                if (reader != null)
                {
                    int index = int.Parse(reader.ReadLine());
                    tile.SetTile(index, m_tileTextures[index]);
                }
                else
                    tile.SetTile((int)EditorDrawMode.DrawEmpty, m_tileTextures[(int)EditorDrawMode.DrawEmpty]);

                tile.onClick.AddListener(delegate { TileButtonClicked(); });
            }
        }
    }

    //Calculates the tile buttons position and width/height based on sceensize.
    private void SetTilePosition(EditorTileButton tile, int x, int y)
    {
        float size = Screen.width / m_rowWidth;
        Vector2 textureLengths = new Vector2(size, size);
        tile.transform.position = new Vector3((size / 2f) + tile.transform.localPosition.x + (textureLengths.x * x), (size / 2f) + tile.transform.localPosition.y + (textureLengths.y * y), 0);
        tile.GetComponent<RectTransform>().sizeDelta = textureLengths;
    }

    //If you press a tile in our map we cache via unity eventsystem. Then we set the tile to current drawed tile.
    private void TileButtonClicked()
    {
        m_lastClickedTile = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<EditorTileButton>();
        m_lastClickedTile.SetTile(m_tileDropDown.value, m_tileTextures[m_tileDropDown.value]);
    }

    private void SaveButtonClicked()
    {
        Save(m_map, m_speed, m_rowWidth, m_rows, m_level);
    }

    private void LoadButtonClicked()
    {
        Load(m_tileTextures);
    }

    private void ClearButtonClicked()
    {
        InitMap(m_rows, DEFAULT_ROW_WIDTH);
    }

    private void PlayButtonClicked()
    {
        //Change to gamestate
		GameState.ContextBase context = new GameState.ContextBase();
		context.ActionOnEnter = GameState.ContextBase.EntryAction.EditorEntry;
        context.Level = m_level;
		Game.Instance.GameState.ChangeState(new InGameState(), context);
    }

    private void RowValueChanged()
    {
        m_rows = int.Parse(m_rowLabel.text);
    }

    private void SpeedValueChanged()
    {
        m_speed = int.Parse(m_speedLabel.text);
    }


    public void Save(MapEditor.Map map, int speed, int mapWidth, int mapHeight, string levelName)
    {
        #if UNITY_EDITOR
        string filePath = UnityEditor.EditorUtility.SaveFilePanel("Save level", "/Assets/Levels/", levelName, "lel");

        if (filePath == null || filePath == "" || filePath.Length == 0)
        {
            return;
        }

        StreamWriter writer = File.CreateText(filePath);
        writer.WriteLine(speed.ToString());
        writer.WriteLine(mapWidth.ToString());
        writer.WriteLine(mapHeight.ToString());

        for (int y = 0; y < mapHeight; ++y)
        {
            for (int x = 0; x < mapWidth; ++x)
            {
                writer.WriteLine(map.Rows[y].Tiles[x].Index);
            }
        }
        writer.Close();
        #endif
    }

    public void Load(Sprite[] textures)
    {
        #if UNITY_EDITOR
        string filePath = UnityEditor.EditorUtility.OpenFilePanel("Load level", "/Assets/Levels/", "lel");
        if (!File.Exists(filePath))
        {
            Debug.LogError("File doesn't exist!");
            return;
        }

        StreamReader reader = File.OpenText(filePath);
        int speed = int.Parse(reader.ReadLine());
        int mapWidth = int.Parse(reader.ReadLine());
        int mapHeight = int.Parse(reader.ReadLine());

        InitMap(mapHeight,mapWidth,reader);
        reader.Close();
        #endif
    }
	public void Load(string filePath)
	{
		#if UNITY_EDITOR
		if (!File.Exists(filePath))
		{
			Debug.LogError("File doesn't exist!");
			return;
		}

		StreamReader reader = File.OpenText(filePath);
		int speed = int.Parse(reader.ReadLine());
		int mapWidth = int.Parse(reader.ReadLine());
		int mapHeight = int.Parse(reader.ReadLine());

		InitMap(mapHeight,mapWidth,reader);
		reader.Close();
		#endif
	}

}
