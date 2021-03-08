using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conduction : MonoBehaviour
{
    [SerializeField]
    private bool positivePassThrough = false, negativePassThrough = false;
    public float voltage, current, resistance;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter!");
        CheckCollisionChange(collision);

    }
    private void OnCollisionStay(Collision collision)
    {
        CheckCollisionChange(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision Exit!");
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
    private void OnTriggerEnter(Collider other)
    {
        CheckTriggerChange(other);

    }
    private void OnTriggerExit(Collider other)
    {
        CheckTriggerChange(other);

    }
    private void OnTriggerStay(Collider other)
    {
        CheckTriggerChange(other);

    }
    private void CurrentCalculator()
    {

    }
    private void CheckCollisionChange(Collision collision)
    {
        if (collision.gameObject.tag == "Power_Source_Positive")
        {
            positivePassThrough = true;
        }
        if (collision.gameObject.tag == "Power_Source_Negative")
        {
            negativePassThrough = true;
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
            if (collision.gameObject.GetComponent<Conduction>().positivePassThrough == true)
            {
                positivePassThrough = true;
            }

        }
    }
    private void CheckTriggerChange(Collider other)
    {
        if (other.gameObject.tag == "Power_Source_Positive")
        {
            positivePassThrough = true;
        }
        if (other.gameObject.tag == "Power_Source_Negative")
        {
            negativePassThrough = true;
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
        if (other.gameObject.GetComponent<Conduction>())
        {
            if (other.gameObject.GetComponent<Conduction>().negativePassThrough == true)
            {
                negativePassThrough = true;
            }
            if (other.gameObject.GetComponent<Conduction>().positivePassThrough == true)
            {
                positivePassThrough = true;
            }

        }
    }
}
