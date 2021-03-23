using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Typical 9 volt battery has 1 amps
*/
public class Battery : MonoBehaviour
{

    [SerializeField] private float powerSourceCurrent = 1;
    public float getPowerSourceCurrent()
    {
        return powerSourceCurrent;
    }

    [SerializeField] private float powerSourceVoltage = 9;
    public float getPowerSourceVoltage()
    {
        return powerSourceVoltage;
    }
}
