using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resistor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float resistorOhms = 4.2f;
    // https://ohmslawcalculator.com/voltage-divider-calculator
    // http://www.gtsparkplugs.com/Dropping_Resistor_Calc.html

// TO DO: READ THE COLOUR OF THE BANDS
  public float getResistorOhms()
    {
        return resistorOhms;
    }
    
}
