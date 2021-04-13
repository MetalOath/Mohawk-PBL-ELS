using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SimulationMethods : Simulation
{
    public void SimulationPlay()
    {
        simulationActiveState = true;
    }
    public void SimulationStop()
    {
        simulationActiveState = false;
    }
    public void SimulationReset()
    {
        SceneManager.LoadScene("_Main_Scene");
    }
    public void ActivateViewMode()
    {
        currentSimulationMode = "ViewMode";
        GameObject.Find("Mode Text (TMP)").GetComponent<TextMeshProUGUI>().text = "View Mode";
    }
    public void ActivateEditMode()
    {
        currentSimulationMode = "EditMode";
        GameObject.Find("Mode Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Edit Mode";
    }
    public void ActivateConnectMode()
    {
        currentSimulationMode = "ConnectMode";
        GameObject.Find("Mode Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Connect Mode";
    }
    public void QuitSimulation()
    {
        Application.Quit();
    }
}
