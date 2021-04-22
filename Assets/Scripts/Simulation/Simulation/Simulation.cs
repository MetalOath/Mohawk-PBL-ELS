using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        Ray nullRay = Camera.main.ScreenPointToRay(Vector3.zero);

        if (platform == "mobile")
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                return raycast;
            }
            else
            {
                return nullRay;
            }
        }
        else if (platform == "desktop")
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                return raycast;
            }
            else
            {
                return nullRay;
            }
        }
        else
        {
            return nullRay;
        }
    }
}
