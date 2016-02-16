using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EditorTileButton : Button
{
    public int Index { get { return m_index; } }

    private int m_index;


    public void SetTile(int index,Sprite sprite)
    {
        if (m_index != index)
        {
            m_index = index;
            ((Image)targetGraphic).sprite = sprite;
        }
    }
}
