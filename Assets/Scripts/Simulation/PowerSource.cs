using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerSource : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        power = false;
        powerSourceVoltage = 0;

        Debug.Log("==Start Method==");
    }

    private bool power { get; set; }

    public int powerSourceVoltage { get; set; }

    public GameObject Battery { get; set; }

    /*
    * 
    */
    public void powerToggle()
    {
        if (power == true)
        {
            power = false;
        }
        else
        {
            power = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("==Update== " + Battery.name);
    }

}
