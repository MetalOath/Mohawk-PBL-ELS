using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimulationEventPublisher : MonoBehaviour
{

    public UnityEvent playSimulation;
    public UnityEvent stopSimulation;
    public UnityEvent resetSimulation;
    public UnityEvent quitSimulation;

    public void PlaySimulation()
    {
        playSimulation?.Invoke();
    }

    public void StopSimulation()
    {
        stopSimulation?.Invoke();
    }

    public void ResetSimulation()
    {
        resetSimulation?.Invoke();
    }
    public void QuitSimulation()
    {
        quitSimulation?.Invoke();
    }
}
