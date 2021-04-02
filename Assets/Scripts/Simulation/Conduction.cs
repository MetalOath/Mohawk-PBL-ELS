using System.Collections;
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
        // TO DO: We used the battery cables for the multimeter and its now seeing the simulation 
        // as an Open loop and will not pass the values for the multimeter to read. 
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
    * The circuit is connnected. Initialize the variables.
    */
    private void ClosedLoopRoutine(Collider other)
    {
        // instance of GameObject that is being collided with 
        GameObject otherObject = other.gameObject;

        // Red wire must touch positive side of power source
        if (otherObject.tag == "9VBatteryPositive" && positiveNumberInSeries == 0)
        {
            positivePassThrough = true;
            positiveNumberInSeries += 1;

            current = otherObject.GetComponent<PowerSource>().getPowerSourceCurrent();
            voltage = otherObject.GetComponent<PowerSource>().getPowerSourceVoltage();

        }
        // Black wire must touch negative side of power source
        if (otherObject.tag == "9VBatteryNegative" && negativeNumberInSeries == 0)
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

        //Debug.Log("closed loop");
        // instance of conduction property of "other" object
        Conduction otherObjectConduction = otherObject.GetComponent<Conduction>();
        if (otherObjectConduction)
        {
   
            // if(positiveNumberInSeries < otherObjectConduction.positiveNumberInSeries){
                if(voltage != 0 && otherObjectConduction.voltage == 0)
                {
                    otherObjectConduction.voltage = voltage;
                }
                if(current != 0 && otherObjectConduction.current == 0)
                {
                    otherObjectConduction.current = current;
                }
                Debug.Log("Voltage: " + positiveNumberInSeries + ": " + voltage);
                Debug.Log("Current: " + positiveNumberInSeries + ": " + current);
                Debug.Log(positiveNumberInSeries + ": " + otherObjectConduction.voltage);
                Debug.Log(positiveNumberInSeries + ": " + otherObjectConduction.current);
            //}
            bool multimeterCheck = otherObject.tag == "multiBlackCable" || otherObject.tag == "multiRedCable";
            //Negative Check
            if (otherObjectConduction.negativePassThrough == true && negativePassThrough == false && !multimeterCheck)
            {
                negativePassThrough = true;
                if (negativeNumberInSeries == 0 && otherObjectConduction.negativeNumberInSeries != 0)
                {
                    negativeNumberInSeries = otherObjectConduction.negativeNumberInSeries + 1;
                }
            }

            //Positive Check
            if (otherObjectConduction.positivePassThrough == true && positivePassThrough == false && !multimeterCheck)
            {
                positivePassThrough = true;
                if (positiveNumberInSeries == 0 && otherObjectConduction.positiveNumberInSeries != 0)
                {
                    positiveNumberInSeries = otherObjectConduction.positiveNumberInSeries + 1;
                }
            }

        }
    }

    /*
    * The circuit is disconnected. Reset all variables.
    */
    private void OpenLoopRoutine()
    {
        //Debug.Log("open loop");
        negativeNumberInSeries = 0;
        negativePassThrough = false;
        positiveNumberInSeries = 0;
        positivePassThrough = false;
        voltage = 0;
        current = 0;
    }
    /*
    * Gets altered by resistor 
    */
    private void CurrentCalculator()
    {

    }

    /*
    * Don't we need this method, this get altered by capacitor?
    */
    private void VoltageCalculator()
    {

    }
}
