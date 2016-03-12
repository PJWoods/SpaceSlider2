using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectMenu : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_scrollViewContentTransform;

    private List<LevelMenuElement> m_elements = new List<LevelMenuElement>();

    private void Start()
    {
        string path = Application.dataPath + "/Levels";
        int index = 0;
        foreach (string file in System.IO.Directory.GetFiles(path))
        {
            string fileName = file;
            string pathToRemove = path + "\\";
            fileName = fileName.Replace(pathToRemove , "");

            if(!fileName.Contains(".meta"))
            {
                fileName = fileName.Replace(".lel", "");
                LevelMenuElement element = Game.Instance.UICore.Create<LevelMenuElement>(Resources.Load("Prefabs/UI/LevelMenuElement") as GameObject);
                element.Initialize(fileName);
                element.transform.parent = m_scrollViewContentTransform.transform;

                RectTransform cachedRect = element.GetComponent<RectTransform>();
                Vector3 pos = cachedRect.localPosition;
                Vector3 newPos = new Vector3(pos.x, (-index * cachedRect.sizeDelta.y) - (cachedRect.sizeDelta.y / 2f), 0);
                element.GetComponent<RectTransform>().localPosition = newPos;

                m_elements.Add(element);

                ++index;
            }
        }
    }

}
