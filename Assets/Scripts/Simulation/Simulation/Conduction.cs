using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conduction : MonoBehaviour
{
    public bool positivePassThrough = false, negativePassThrough = false, simulationActiveState = false, loopIsClosed = false;
    public float voltage, current, resistance, localResistance;
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

            voltage = otherObject.GetComponent<PowerSource>().getPowerSourceVoltage();

        }
        // Black wire must touch negative side of power source
        if (otherObject.tag == "Power_Source_Negative" && negativeNumberInSeries == 0)
        {
            negativePassThrough = true;
            negativeNumberInSeries += 1;
        }

        if (otherObject.tag == "Power_Source_Negative" && current == 0)
        {
            current = otherObject.GetComponent<PowerSource>().getCurrent();
            //Debug.Log("getting battery current");
        }
        // instance of conduction property of "other" object
        Conduction otherObjectConduction = otherObject.GetComponent<Conduction>();
        if (otherObjectConduction)
        {
            if(positiveNumberInSeries > otherObjectConduction.positiveNumberInSeries)
            {
                if(otherObjectConduction.voltage != 0 && voltage == 0)
                {
                    voltage = otherObjectConduction.voltage;
                }
                if (otherObjectConduction.resistance != 0 && resistance == 0)
                {
                    resistance = otherObjectConduction.resistance + localResistance;
                    //Debug.Log("getting resistance from resistor");
                }
                if (otherObjectConduction.resistance == 0 && resistance == 0 && localResistance != 0)
                {
                    resistance = localResistance;
                    //Debug.Log("Setting local resistance");
                }
            }

            if(negativeNumberInSeries > otherObjectConduction.negativeNumberInSeries)
            {
                if (otherObjectConduction.current != 0 && current == 0)
                {
                    current = otherObjectConduction.current;
                }
            }

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
}