using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationMethods : MonoBehaviour
{
    public bool simulationActiveState = false;
    public string currentGameMode = "ViewMode";
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
    public void GetCurrentGameMode()
    {
        if (GameObject.Find("ViewMode Canvas").activeInHierarchy)
        {
            currentGameMode = "ViewMode";
        }
        if (GameObject.Find("EditMode Canvas").activeInHierarchy)
        {
            currentGameMode = "EditMode";
        }
        if (GameObject.Find("ConnectMode Canvas").activeInHierarchy)
        {
            currentGameMode = "ConnectMode";
        }
    }
}
