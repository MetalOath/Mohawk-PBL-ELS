﻿using System.Collections;
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
        // TODO: FIX MOBILE UI RAYCAST BLOCKING
        if (platform == "mobile")
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (!IsPointerOverGameObject())
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

    //testing mobile UI raycast block.
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    /// <returns>true if mouse or first touch is over any event system object ( usually gui elements )</returns>
    public static bool IsPointerOverGameObject()
    {
        //check mouse
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        //check touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return true;
        }

        return false;
    }
}
