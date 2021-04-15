using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Simulation : MonoBehaviour
{
    public bool simulationActiveState = false;
    public string currentSimulationMode;

    public bool inWireSpawnPhase = false;

    public string platform;

    public void Start()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            platform = "mobile";
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            platform = "desktop";
        }
    }

    public Ray SingleRayCastByPlatform()
    {
        if (platform == "mobile")
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            return raycast;
        }
        else if (platform == "desktop")
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            return raycast;
        }
        else
        {
            Ray raycast = Camera.main.ScreenPointToRay(Vector3.zero);
            return raycast;
        }
    }
}
