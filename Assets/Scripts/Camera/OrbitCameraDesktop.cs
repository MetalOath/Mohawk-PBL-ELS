using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCameraDesktop : OrbitCamera
{

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
    private void OnEnable()
    {
        this.enabled = false;
    }
#endif

    public bool allowMiddleMouse;
    public bool allowLeftMouse;
    public bool allowRightMouse;
    public bool allowScroll;

    private float touchTime;

    public override void UserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchTime = Time.time;
        }

        if (allowLeftMouse && Input.GetMouseButton(0))
        {
            PerformRotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        if (allowRightMouse && Input.GetMouseButton(1))
        {
            PerformPan(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        
        if (allowMiddleMouse && Input.GetMouseButton(2))
        {
            PerformRotate(Input.GetAxis("Mouse X"),0);
        }

        if (allowScroll && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            PerformZoom(Input.GetAxis("Mouse ScrollWheel"));
        }

        if (currentGameMode == "ViewMode" && (Time.time - touchTime) < 0.2f)
        {
            ViewModeZoomToComponentDesktop();
        }
    }

    private void ViewModeZoomToComponentDesktop()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("Interactive"))
                {
                    ViewModeZoomToComponent(raycastHit.collider.transform);
                }
            }
        }
    }
}
