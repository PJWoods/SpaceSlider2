using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Button m_playButtonClicked;

    [SerializeField]
    private TweenPosition[] m_anchors;

    [SerializeField]
    private TweenAlpha m_playTextAlphaTween;

    void Start ()
    {
        AnimateAnchors(true);
        m_playButtonClicked.onClick.AddListener(delegate { PlayButtonClicked(); });
    }

	void PlayButtonClicked()
    {
        m_playTextAlphaTween.Play(true);
        AnimateAnchors(false);
        CoroutineHandler.Run(YieldAnimationThenContinue(1f));
	}

    IEnumerator YieldAnimationThenContinue(float yieldTime)
    {
        yield return new WaitForSeconds(yieldTime);
        Game.Instance.GameState.ChangeState(new InGameState());
    }

    void AnimateAnchors(bool forward)
    {
        for (int i = 0; i < m_anchors.Length; i++)
        {
            m_anchors[i].Play(forward);
        }
    }
}
