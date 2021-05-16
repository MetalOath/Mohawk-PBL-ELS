using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : MonoBehaviour
{    
    [SerializeField] private float powerSourceVoltage, calculatedCurrent, circuitResistanceTotal, powerSourceMaxCurrent;

    private string terminal;

    void Start()
    {
        terminal = gameObject.tag;
    }

    private void OnTriggerStay(Collider other)
    {
        Conduction otherObjectConduction = other.GetComponent<Conduction>();
        if (otherObjectConduction)
        {
            if (terminal == "Power_Source_Positive" && otherObjectConduction.loopIsClosed)
            {

            }
            if (terminal == "Power_Source_Negative" && otherObjectConduction.loopIsClosed && circuitResistanceTotal == 0f)
            {
                circuitResistanceTotal = otherObjectConduction.resistance;
            }
        }
    }

    public float getCurrent()
    {
        if (circuitResistanceTotal != 0f)
        {
            calculatedCurrent = powerSourceVoltage / circuitResistanceTotal;
        }
        return calculatedCurrent;
    }
    
    public float getPowerSourceVoltage()
    {
        return powerSourceVoltage;
    }
}