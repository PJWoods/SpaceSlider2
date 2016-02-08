using UnityEngine;
using System.Collections;

public class InitializeStartUp : MonoBehaviour
{
    void Start()
    {
        Game.Instance.GameStartUp();
    }
}
