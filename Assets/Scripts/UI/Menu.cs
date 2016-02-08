using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Button m_playButtonClicked;

    void Start ()
    {
        m_playButtonClicked.onClick.AddListener(delegate { PlayButtonClicked(); });
    }

	void PlayButtonClicked()
    {
        Game.Instance.GameState.ChangeState(new InGameState());
	}
}
