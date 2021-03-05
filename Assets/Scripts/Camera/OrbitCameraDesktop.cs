using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCameraDesktop : OrbitCamera
{
    public bool allowMiddleMouse;
    public bool allowLeftMouse;
    public bool allowRightMouse;
    public bool allowScroll;

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
    private void OnEnable()
    {
        this.enabled = false;
    }
#endif


    public override void UserInput()
    {
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
    }
}
