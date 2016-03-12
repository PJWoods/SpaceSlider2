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

        LevelSelectMenu menu = Game.Instance.UICore.Create<LevelSelectMenu>(Resources.Load("Prefabs/UI/LevelSelectMenu") as GameObject);
    }

    IEnumerator YieldAnimationThenContinueToEditor(float yieldTime)
    {
        yield return new WaitForSeconds(yieldTime);

		GameState.ContextBase context = new GameState.ContextBase();
		context.ActionOnEnter = GameState.ContextBase.EntryAction.GameEntry;
		context.Level = "default";
		Game.Instance.GameState.ChangeState(new EditorState(), context);
    }

    void AnimateAnchors(bool forward)
    {
        for (int i = 0; i < m_anchors.Length; i++)
        {
            m_anchors[i].Play(forward);
        }
    }
}
