using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationMethods : MonoBehaviour
{
    public GameObject breadboard, multimeter, multimeterScreen, resistor, battery, led, ledLight; 
    public void CheckConnection()
    {
        if (breadboard.activeInHierarchy && resistor.activeInHierarchy && led.activeInHierarchy && battery.activeInHierarchy)
        {
            ledLight.SetActive(true);
        }
        else
        {
            ledLight.SetActive(false);
        }

        if (breadboard.activeInHierarchy && battery.activeInHierarchy)
        {
            multimeterScreen.SetActive(true);
        }
        else
        {
            multimeterScreen.SetActive(false);
        }
    }
    public void ResetSimulation()
    {
        SceneManager.LoadScene("_Main_Scene");
    }
    public void ConductCurrent()
    {

    }
}
