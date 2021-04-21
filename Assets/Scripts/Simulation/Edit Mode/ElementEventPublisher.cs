using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElementEventPublisher : MonoBehaviour
{
    public UnityEvent ElementMethods;

    public void InvokeElementMethods()
    {
        ElementMethods?.Invoke();
    }
}
