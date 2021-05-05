using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventMethods : MonoBehaviour
{
    List<GameObject> UICanvases = new List<GameObject>();
    public List<GameObject> connectionPoints = new List<GameObject>();
    public List<GameObject> selectionPoints = new List<GameObject>();
    public List<GameObject> spawnColliders = new List<GameObject>();
    GameObject[] allGameObjects;

    SimulationMethods Simulation;
    WireInstantiator WireInstantiator;

    [SerializeField] private GameObject wirePrompt, wireSelectUI, wireSelectUIZoomed;

    private void Start()
    {
        Simulation = GameObject.Find("Simulation Event Handler").GetComponent<SimulationMethods>();
        WireInstantiator = GameObject.Find("Simulation Event Handler").GetComponent<WireInstantiator>();
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
        if (Simulation.currentSimulationMode == "ConnectMode" && WireInstantiator.leadSpawnPhase && wireSelectUI.activeInHierarchy)
        {
            wireSelectUI.SetActive(false);
            wireSelectUIZoomed.SetActive(false);
        }else if (!wireSelectUI.activeInHierarchy && !WireInstantiator.leadSpawnPhase)
        {
            wireSelectUI.SetActive(true);
            wireSelectUIZoomed.SetActive(true);
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
    private void PopulateSCList()
    {
        foreach (GameObject SC in allGameObjects)
        {
            if (SC.CompareTag("SpawnCollider"))
            {
                spawnColliders.Add(SC);
            }
        }
    }
    public void UpdateGameObjectList()
    {
        connectionPoints.Clear();
        selectionPoints.Clear();
        spawnColliders.Clear();
        allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        PopulateCPList();
        PopulateSPList();
        PopulateSCList();
    }
    public void ClearUI()
    {
        foreach (GameObject UICanvas in UICanvases)
        {
            UICanvas.SetActive(false);
        }
    }
}
