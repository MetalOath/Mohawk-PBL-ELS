using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDLight : MonoBehaviour
{
    private bool n, p;
    public GameObject ledLight;

    /*
    * Updates every frame. 
    * When the negativePassThrough and posistivePassThrough is switched to true, then turn on the LED light
    */
    private void Update()
    {
        n = gameObject.GetComponent<Conduction>().negativePassThrough;
        p = gameObject.GetComponent<Conduction>().positivePassThrough;
        //Checks to see if the condition is true 
        if (n && p)
        {
            //Turn on the light
            ledLight.SetActive(true);
        }
        else
        {
            //Turn off the light
            ledLight.SetActive(false);
        }
    }
}
