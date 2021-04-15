using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conduction : MonoBehaviour
{
    public bool positivePassThrough = false, negativePassThrough = false, simulationActiveState = false, loopIsClosed = false;
    public float voltage, current, resistance;
    private SimulationMethods Simulation;
    /*
    * To know when a parallel circuit is created. 
    * This will show where in the circuit it was split 
    * to pass the current values from that point forward.
    */
    public int positiveNumberInSeries, negativeNumberInSeries;

    public bool LoopIsClosed()
    {
        return loopIsClosed;
    }

    private void Start()
    {
        Simulation = GameObject.Find("Simulation Event Handler").GetComponent<SimulationMethods>();
    }
    /**
     * Starts the electrical circuit when the simulation state is active.
     * Electrical circuit will pass all the properties it needs to mimic current, 
     * voltage, and resistance passing through an electrical component.
     */
    private void OnTriggerStay(Collider other)
    {
        // TO DO: We used the battery cables for the multimeter and its now seeing the simulation 
        // as an Open loop and will not pass the values for the multimeter to read. 
        simulationActiveState = Simulation.simulationActiveState;
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
        if (positivePassThrough == true && negativePassThrough == true)
        {
            loopIsClosed = true;
        }
        // instance of GameObject that is being collided with 
        GameObject otherObject = other.gameObject;

        // Red wire must touch positive side of power source
        if (otherObject.tag == "Power_Source_Positive" && positiveNumberInSeries == 0)
        {
            positivePassThrough = true;
            positiveNumberInSeries += 1;

            current = otherObject.GetComponent<PowerSource>().getPowerSourceCurrent();
            voltage = otherObject.GetComponent<PowerSource>().getPowerSourceVoltage();

        }
        // Black wire must touch negative side of power source
        if (otherObject.tag == "Power_Source_Negative" && negativeNumberInSeries == 0)
        {
            negativePassThrough = true;
            negativeNumberInSeries += 1;
        }
        // Red wire must touch positive side of power source
        if (otherObject.tag == "ResistorBody")
        {
            // Debug.Log("REACHED RESISTOR BODY TAG");
            positivePassThrough = true;
            positiveNumberInSeries += 1;

            resistance = otherObject.GetComponent<Resistor>().getResistorOhms();
            //Debug.Log("Resistance: " + resistance);
            CurrentCalculator();
        }
        // if(otherObject.tag == "LedWire"){
        // }

        // instance of conduction property of "other" object
        Conduction otherObjectConduction = otherObject.GetComponent<Conduction>();
        if (otherObjectConduction)
        {
            // To Do: Check for the this conduction script to be less than the other conduction script in the series.
            // To Do: positivenumberinseries is increasing alot.
            // if(positiveNumberInSeries < otherObjectConduction.positiveNumberInSeries){
                if(voltage != 0 && otherObjectConduction.voltage == 0)
                {
                    otherObjectConduction.voltage = voltage;
                }
                if(current != 0 && otherObjectConduction.current == 0)
                {
                    otherObjectConduction.current = current;
                }
                if(resistance != 0 && otherObjectConduction.resistance == 0)
                {
                    otherObjectConduction.resistance = resistance;
                }
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
        negativeNumberInSeries = 0;
        negativePassThrough = false;
        positiveNumberInSeries = 0;
        positivePassThrough = false;
        voltage = 0;
        current = 0;
        resistance = 0;
        loopIsClosed = false;
    }
    /*
    * Gets altered by resistor 
    */
    private void CurrentCalculator()
    {       
        // Debug.Log("CURRENT: " + current);
        // Debug.Log("VOLTAGE: " + voltage);

        // To Do: Current isn't being seen by the multimeter.
        current = voltage / resistance;
    }
}