using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conduction : MonoBehaviour
{
    public bool positivePassThrough = false, negativePassThrough = false, closedLoop = false;
    public float voltage, current, resistance;
    public int positiveNumberInSeries, negativeNumberInSeries;
    /*private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision Enter!");
        CheckCollisionChange(collision);

    }
    private void OnCollisionStay(Collision collision)
    {
        CheckCollisionChange(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log("Collision Exit!");
        if (collision.gameObject.tag == "Power_Source")
        {

        }
        if (collision.gameObject.tag == "Resistor")
        {

        }
        if (collision.gameObject.tag == "LED_Light")
        {

        }
        if (collision.gameObject.tag == "Wire")
        {

        }
        CheckCollisionChange(collision);
    }
    private void CheckCollisionChange(Collision collision)
    {
        if (collision.gameObject.tag == "Power_Source_Positive")
        {
            positivePassThrough = true;
        }
        else
        {
            //positivePassThrough = false;
        }
        if (collision.gameObject.tag == "Power_Source_Negative")
        {
            negativePassThrough = true;
        }
        else
        {
            //negativePassThrough = false;
        }
        if (collision.gameObject.tag == "Resistor")
        {

        }
        if (collision.gameObject.tag == "LED_Light")
        {

        }
        if (collision.gameObject.tag == "Wire")
        {

        }
        if (collision.gameObject.GetComponent<Conduction>())
        {
            if (collision.gameObject.GetComponent<Conduction>().negativePassThrough == true)
            {
                negativePassThrough = true;
            }
            else
            {
                //negativePassThrough = false;
            }
            if (collision.gameObject.GetComponent<Conduction>().positivePassThrough == true)
            {
                positivePassThrough = true;
            }
            else
            {
                //positivePassThrough = false;
            }
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Power_Source_Positive")
        {
            positivePassThrough = true;
            positiveNumberInSeries += 1;
        }
        if (other.gameObject.tag == "Power_Source_Negative")
        {
            negativePassThrough = true;
            negativeNumberInSeries += 1;
        }
        if (other.gameObject.tag == "Resistor")
        {

        }
        if (other.gameObject.tag == "LED_Light")
        {

        }
        if (other.gameObject.tag == "Wire")
        {

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Power_Source_Positive")
        {
            positivePassThrough = false;
            positiveNumberInSeries = 0;
        }
        if (other.gameObject.tag == "Power_Source_Negative")
        {
            negativePassThrough = false;
            negativeNumberInSeries = 0;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        ClosedLoopRoutine(other);
    }
    private void ClosedLoopRoutine(Collider other)
    {
        if (other.gameObject.GetComponent<Conduction>() && closedLoop == false)
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
        if (other.gameObject.tag == "Power_Source_Positive" || other.gameObject.tag == "Power_Source_Negative")
        {
            closedLoop = other.gameObject.GetComponent<Battery>().closedLoop;
        }
    }
    private void OpenLoopRoutine(Collider other)
    {
        if (other.gameObject.GetComponent<Conduction>() && closedLoop == true)
        {
            if (other.gameObject.GetComponent<Conduction>().negativeNumberInSeries == negativeNumberInSeries - 1 && other.gameObject.GetComponent<Conduction>().negativePassThrough == false)
            {
                negativeNumberInSeries = 0;
                negativePassThrough = false;
            }
            if (other.gameObject.GetComponent<Conduction>().positiveNumberInSeries == positiveNumberInSeries - 1 && other.gameObject.GetComponent<Conduction>().positivePassThrough == false)
            {
                positiveNumberInSeries = 0;
                positivePassThrough = false;
            }
        }
    }
    private void CurrentCalculator()
    {

    }
}
