using UnityEngine;
using System.Collections;

public class InGameState : GameState
{
    public override void Begin()
    {
        Game.Instance.LoadLevel("assets");
    }

    public override void Update()
    {

    }

    public override void End()
    {
        Game.Instance.UICore.Clear();
    }
}
