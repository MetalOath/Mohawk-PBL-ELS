using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIEventPublisher : MonoBehaviour
{
    public UnityEvent activateViewModeUI;
    public UnityEvent activateViewModeZoomedUI;
    public UnityEvent activateEditModeUI;
    public UnityEvent activateEditModeZoomedUI;
    public UnityEvent activateConnectModeUI;

    public void ViewModeUI()
    {
        activateViewModeUI?.Invoke();
    }
    public void ViewModeZoomedUI()
    {
        activateViewModeZoomedUI?.Invoke();
    }
    public void EditModeUI()
    {
        activateEditModeUI?.Invoke();
    }
    public void EditModeZoomedUI()
    {
        activateEditModeZoomedUI?.Invoke();
    }
    public void ConnectModeUI()
    {
        activateConnectModeUI?.Invoke();
    }
}
