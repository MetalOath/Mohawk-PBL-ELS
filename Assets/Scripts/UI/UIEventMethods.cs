using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventMethods : MonoBehaviour
{
    List<GameObject> UICanvases = new List<GameObject>();
    public List<GameObject> connectionPoints = new List<GameObject>();
    public List<GameObject> selectionPoints = new List<GameObject>();
    GameObject[] allGameObjects;

    SimulationMethods Simulation;

    [SerializeField] private GameObject wirePrompt;

    private void Start()
    {
        Simulation = GameObject.Find("Simulation Event Handler").GetComponent<SimulationMethods>();
        allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        PopulateUICanvasList();
        PopulateCPList();
        PopulateSPList();
    }

    private void Update()
    {
        if (Simulation.currentSimulationMode == "ConnectMode" && !wirePrompt.activeInHierarchy)
        {
            wirePrompt.SetActive(true);
        }
        else if (Simulation.currentSimulationMode != "ConnectMode" && wirePrompt.activeInHierarchy)
        {
            wirePrompt.SetActive(false);
        }
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
    private void PopulateCPList()
    {
        foreach (GameObject CP in allGameObjects)
        {
            if (CP.CompareTag("Connection_Points"))
            {
                connectionPoints.Add(CP);
            }
        }
    }
    private void PopulateSPList()
    {
        foreach (GameObject SP in allGameObjects)
        {
            if (SP.CompareTag("Selection_Points"))
            {
                selectionPoints.Add(SP);
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
