﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationMethods : MonoBehaviour
{
    public bool simulationActiveState = false;
    public string currentSimulationMode = "ViewMode";
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
    }
    public void ActivateEditMode()
    {
        currentSimulationMode = "EditMode";
    }
    public void ActivateConnectMode()
    {
        currentSimulationMode = "ConnectMode";
    }
}
