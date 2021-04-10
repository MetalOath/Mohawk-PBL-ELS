using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrbitCameraEventPublisher : MonoBehaviour
{
    public UnityEvent activateViewModeCamera;
    public UnityEvent activateViewModeZoomedCamera;
    public UnityEvent activateEditModeCamera;
    public UnityEvent activateConnectModeCamera;

    public void ViewModeCamera()
    {
        activateViewModeCamera?.Invoke();
    }
    public void ViewModeZoomedCamera()
    {
        activateViewModeZoomedCamera?.Invoke();
    }
    public void EditModeCamera()
    {
        activateEditModeCamera?.Invoke();
    }
    public void ConnectModeCamera()
    {
        activateConnectModeCamera?.Invoke();
    }
}