using System;
using UnityEngine;

public class MenuState : GameState
{
    public override void Begin()
    {
        //Game.Instance.UICore.Create<Menu>(Resources.Load("Prefabs/GUI/Menu") as GameObject);
    }

    public override void Update()
    {

    }

    public override void End()
    {
        Game.Instance.UICore.Clear();
    }
}