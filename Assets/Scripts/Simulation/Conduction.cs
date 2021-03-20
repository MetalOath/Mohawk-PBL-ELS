﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conduction : MonoBehaviour
{
    public bool positivePassThrough = false, negativePassThrough = false, simulationActiveState = false;
    public float voltage, current, resistance;
    /*
    * To know when a parallel circuit is created. 
    * This will show where in the circuit it was split 
    * to pass the current values from that point forward.
    */
    public int positiveNumberInSeries, negativeNumberInSeries;

    /**
     * Starts the electrical circuit when the simulation state is active.
     * Electrical circuit will pass all the properties it needs to mimic current, 
     * voltage, and resistance passing through an electrical component.
     */
    private void OnTriggerStay(Collider other)
    {
        simulationActiveState = GameObject.Find("Simulation Event Handler").GetComponent<SimulationMethods>().simulationActiveState;
        if (simulationActiveState == true)
        {
            ClosedLoopRoutine(other);
        }
        else if (simulationActiveState == false)
        {
            OpenLoopRoutine();
        }
    }

    /*
    *
    *
    */
    private void ClosedLoopRoutine(Collider other)
    {
        // instance of GameObject that is being collided with 
        GameObject otherObject = other.gameObject;

        if (otherObject.tag == "Power_Source_Positive" && positiveNumberInSeries == 0)
        {
            positivePassThrough = true;
            positiveNumberInSeries += 1;
        }
        if (otherObject.tag == "Power_Source_Negative" && negativeNumberInSeries == 0)
        {
            negativePassThrough = true;
            negativeNumberInSeries += 1;
        }

        //switch (otherObject.tag)
        //{
        //    case "Resistor":
        //        break;
        //    case "LED_Light":
        //        break;
        //    case "Wire":
        //        break;
        //    default:
        //        break;
        //}


        //if (other.gameObject.tag == "Resistor")
        //{

        //}
        //if (other.gameObject.tag == "LED_Light")
        //{

        //}
        //if (other.gameObject.tag == "Wire")
        //{

        //}

        // instance of conduction property of "other" object
        Conduction otherObjectConduction = otherObject.GetComponent<Conduction>();
        if (otherObjectConduction)
        {

            //Negative Check
            if (otherObjectConduction.negativePassThrough == true && negativePassThrough == false)
            {
                negativePassThrough = true;
                if (negativeNumberInSeries == 0 && otherObjectConduction.negativeNumberInSeries != 0)
                {
                    negativeNumberInSeries = otherObjectConduction.negativeNumberInSeries + 1;
                }
            }

            //Positive Check
            if (otherObjectConduction.positivePassThrough == true && positivePassThrough == false)
            {
                positivePassThrough = true;
                if (positiveNumberInSeries == 0 && otherObjectConduction.positiveNumberInSeries != 0)
                {
                    positiveNumberInSeries = otherObjectConduction.positiveNumberInSeries + 1;
                }
            }
        }
    }

    private void OpenLoopRoutine()
    {
        negativeNumberInSeries = 0;
        negativePassThrough = false;
        positiveNumberInSeries = 0;
        positivePassThrough = false;
    }

    private void CurrentCalculator()
    {

    }
}
