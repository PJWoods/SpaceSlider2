using UnityEngine;
using System.Collections;

public class InGameState : GameState
{
    public enum EntryAction
    {
        EditorEntry,
    }

    public class Context : ContextBase
    {
        public EntryAction EntryAction;
        public string Level;
    }

    public EntryAction Action { get { return m_entryAction; } }

    private EntryAction m_entryAction;

    public override void Begin(ContextBase context)
    {
        Context castedContext = context as Context;
        Game.Instance.LoadLevel("Game");
    }

    public override void Update()
    {

    }

    public override void End()
    {
        Game.Instance.UICore.Clear();
    }
}
