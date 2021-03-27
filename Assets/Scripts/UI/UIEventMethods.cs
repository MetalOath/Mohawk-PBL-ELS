using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventMethods : MonoBehaviour
{
    [SerializeField] List<GameObject> UICanvases = new List<GameObject>();
    GameObject[] allGameObjects;

    private void Start()
    {
        allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        PopulateUICanvasList();
    }
    private void PopulateUICanvasList()
    {
        foreach (GameObject UIElement in allGameObjects)
        {
            if (UIElement.CompareTag("UI_Canvas"))
            {
                UICanvases.Add(UIElement);
            }
        }
    }
    public void ClearUI()
    {
        foreach (GameObject UICanvas in UICanvases)
        {
            UICanvas.SetActive(false);
        }
    }
}
