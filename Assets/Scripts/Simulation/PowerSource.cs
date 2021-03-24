using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Typical 9 volt battery has 1 amps
*/
public class PowerSource : MonoBehaviour
{
    [SerializeField] private int powerSourceCurrent = 1;
    public int getPowerSourceCurrent()
    {
        return powerSourceCurrent;
    }

    [SerializeField] private int powerSourceVoltage = 9;
    public int getPowerSourceVoltage()
    {
        return powerSourceVoltage;
    }
}
