using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelMenuElement : MonoBehaviour
{
    enum ColorPresets
    {
        Orange,
        Purple,
        Green,
    }

    [SerializeField]
    private Text m_nameLabel;

    [SerializeField]
    private Color[] m_colorPresets;

    [SerializeField]
    private GameObject[] m_levelSetups;

    private string m_levelName;

    public void Initialize(string mapName)
    {
        m_levelName = mapName;
        m_nameLabel.text = m_levelName;

        int random = Random.Range(0,Mathf.Min(m_levelSetups.Length, m_colorPresets.Length));
        m_levelSetups[random].SetActive(true);
        GetComponent<Image>().color = m_colorPresets[random];

        GetComponent<Button>().onClick.AddListener(delegate { Pressed(); });
    }

    private void Pressed()
    {
        InGameState.ContextBase context = new GameState.ContextBase();
        context.Level = m_levelName;
        context.ActionOnEnter = GameState.ContextBase.EntryAction.GameEntry;
        Game.Instance.GameState.ChangeState(new InGameState(), context);
    }
}
