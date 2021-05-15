using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultimeterPort : MonoBehaviour
{
    public float voltageReading, currentReading, resistanceReading;
    public bool isConnected = false;

    private void OnTriggerStay(Collider other)
    {
        Conduction otherObjectConduction = other.GetComponentInParent<Conduction>();
        if (otherObjectConduction)
        {
            voltageReading = otherObjectConduction.voltage;
            currentReading = otherObjectConduction.current;
            resistanceReading = otherObjectConduction.resistance;
            isConnected = true;
        }
    }
}