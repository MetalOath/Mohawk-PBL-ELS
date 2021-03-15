using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationMethods : MonoBehaviour
{
    public bool simulationActiveState = false;
    public void PlayPauseSimulation()
    {
        SimulationToggle();
    }
    public void ResetSimulation()
    {
        SceneManager.LoadScene("_Main_Scene");
    }
    public void SimulationStop()
    {
        simulationActiveState = false;
    }
    private void SimulationToggle()
    {
        simulationActiveState = !simulationActiveState;
    }
}
