using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventMethods : MonoBehaviour
{
    [SerializeField] private GameObject wirePrompt, wireSelectUI, wireSelectUIZoomed;

    SimulationMethods Simulation;

    private void Start()
    {
        Simulation = GameObject.Find("Simulation Event Handler").GetComponent<SimulationMethods>();
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
        if (Simulation.currentSimulationMode == "ConnectMode" && Simulation.WireInstantiator.leadSpawnPhase && wireSelectUI.activeInHierarchy)
        {
            wireSelectUI.SetActive(false);
            wireSelectUIZoomed.SetActive(false);
        }
        else if (!wireSelectUI.activeInHierarchy && !Simulation.WireInstantiator.leadSpawnPhase)
        {
            wireSelectUI.SetActive(true);
            wireSelectUIZoomed.SetActive(true);
        }
    }

    public void ClearUI()
    {
        foreach (GameObject UICanvas in Simulation.UICanvases)
        {
            UICanvas.SetActive(false);
        }
    }
}
