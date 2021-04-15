using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Typical 9 volt battery has a 1 amps limit
*/
public class PowerSource : MonoBehaviour
{    
    [SerializeField] private float powerSourceVoltage, powerSourceMaxCurrent;

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
            else if (terminal == "Power_Source_Negative" && otherObjectConduction.loopIsClosed)
            {

            }
        }
    }

    public float getPowerSourceCurrent()
    {
        return powerSourceMaxCurrent;
    }
    
    public float getPowerSourceVoltage()
    {
        return powerSourceVoltage;
    }
}