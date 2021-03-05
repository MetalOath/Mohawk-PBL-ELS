using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimulationEventPublisher : MonoBehaviour
{
    public UnityEvent playSimulation;
    public UnityEvent resetSimulation;

    public void PlaySimulation()
    {
        playSimulation?.Invoke();
    }
    public void ResetSimulation()
    {
        resetSimulation?.Invoke();
    }
}
