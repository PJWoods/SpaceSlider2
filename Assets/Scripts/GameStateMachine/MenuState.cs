using System;
using UnityEngine;

public class MenuState : GameState
{
    public override void Begin(ContextBase context)
    {
        Game.Instance.UICore.Create<Menu>(Resources.Load("Prefabs/UI/Menu") as GameObject);
    }

    public override void Update()
    {

    }

    public override void End()
    {
        Game.Instance.UICore.Clear();
    }
}