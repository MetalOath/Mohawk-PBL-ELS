using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrbitCameraEventPublisher : MonoBehaviour
{
    public UnityEvent activateViewModeCamera;
    public UnityEvent activateViewModeZoomedCamera;
    public UnityEvent activateEditModeCamera;
    public UnityEvent activateEditModeZoomedCamera;
    public UnityEvent activateConnectModeCamera;
    public UnityEvent activateConnectModeZoomedCamera;

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
    public void EditModeZoomedCamera()
    {
        activateEditModeZoomedCamera?.Invoke();
    }
    public void ConnectModeCamera()
    {
        activateConnectModeCamera?.Invoke();
    }
    public void ConnectModeZoomedCamera()
    {
        activateConnectModeZoomedCamera?.Invoke();
    }
}