using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Typical 9 volt battery has 1 amps
*/
public class PowerSource : MonoBehaviour
{    
    [SerializeField] private float powerSourceCurrent;
    [SerializeField] private float powerSourceVoltage;
    void Start(){
        PowerSource ps = gameObject.GetComponent<PowerSource>();
        string gt = ps.tag;
        Debug.Log(gt);
        if(gt == "9VBatteryPositive" || gt == "9VBatteryNegative")
        {
        powerSourceCurrent = 1f;
        powerSourceVoltage = 9f;
        }
        else if(gt == "PotatoBattery"){
            // do something
        }else if(gt == "DCPowerSupplyPositive" || gt == "DCPowerSupplyNegative"){
            // do something
        }
    }
    public float getPowerSourceCurrent()
    {
        return powerSourceCurrent;
    }

   
    public float getPowerSourceVoltage()
    {
        return powerSourceVoltage;
    }
}
