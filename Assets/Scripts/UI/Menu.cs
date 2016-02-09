using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Button m_playButtonClicked;

    [SerializeField]
    private Button m_editorButtonClicked;

    [SerializeField]
    private TweenPosition[] m_anchors;

    [SerializeField]
    private TweenAlpha m_playTextAlphaTween;

    void Start ()
    {
        AnimateAnchors(true);
        m_playButtonClicked.onClick.AddListener(delegate { PlayButtonClicked(); });
        m_editorButtonClicked.onClick.AddListener(delegate { EditorButtonClicked(); });
    }

    void EditorButtonClicked()
    {
        m_playTextAlphaTween.Play(true);
        AnimateAnchors(false);
        CoroutineHandler.Run(YieldAnimationThenContinueToEditor(1f));
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

    IEnumerator YieldAnimationThenContinueToEditor(float yieldTime)
    {
        yield return new WaitForSeconds(yieldTime);
        Game.Instance.GameState.ChangeState(new EditorState());
    }

    void AnimateAnchors(bool forward)
    {
        for (int i = 0; i < m_anchors.Length; i++)
        {
            m_anchors[i].Play(forward);
        }
    }
}
