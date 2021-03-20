using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationMethods : MonoBehaviour
{
    public bool simulationActiveState = false;

    /*
    * When the user clicks on the Play button, play the simulation
    */
    public void SimulationPlay()
    {
        simulationActiveState = true;
    }

    /*
    * When the user clicks on the Pause button, pause the simulation 
    */
    public void SimulationStop()
    {
        simulationActiveState = false;
    }

    /*
    * When the user clicks on the Reset button, resets the scene to its original state 
    */
    public void SimulationReset()
    {
        SceneManager.LoadScene("_Main_Scene");
    }
}
