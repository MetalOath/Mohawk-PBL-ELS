using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conduction : MonoBehaviour
{
    public bool positivePassThrough = false, negativePassThrough = false, simulationActiveState = false;
    public float voltage, current, resistance;
    public int positiveNumberInSeries, negativeNumberInSeries;

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
    private void ClosedLoopRoutine(Collider other)
    {
        if (other.gameObject.tag == "Power_Source_Positive" && positiveNumberInSeries == 0)
        {
            positivePassThrough = true;
            positiveNumberInSeries += 1;
        }
        if (other.gameObject.tag == "Power_Source_Negative" && negativeNumberInSeries == 0)
        {
            negativePassThrough = true;
            negativeNumberInSeries += 1;
        }
        /*if (other.gameObject.tag == "Resistor")
        {

        }
        if (other.gameObject.tag == "LED_Light")
        {

        }
        if (other.gameObject.tag == "Wire")
        {

        }*/
        if (other.gameObject.GetComponent<Conduction>())
        {
            //Negative Check
            if (other.gameObject.GetComponent<Conduction>().negativePassThrough == true && negativePassThrough == false)
            {
                negativePassThrough = true;
                if (negativeNumberInSeries == 0 && other.gameObject.GetComponent<Conduction>().negativeNumberInSeries != 0)
                {
                    negativeNumberInSeries = other.gameObject.GetComponent<Conduction>().negativeNumberInSeries + 1;
                }
            }
            //Positive Check
            if (other.gameObject.GetComponent<Conduction>().positivePassThrough == true && positivePassThrough == false)
            {
                positivePassThrough = true;
                if (positiveNumberInSeries == 0 && other.gameObject.GetComponent<Conduction>().positiveNumberInSeries != 0)
                {
                    positiveNumberInSeries = other.gameObject.GetComponent<Conduction>().positiveNumberInSeries + 1;
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
