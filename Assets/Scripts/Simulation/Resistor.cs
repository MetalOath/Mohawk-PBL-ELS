using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resistor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float resistorOhms;

// TO DO: READ THE COLOUR OF THE BANDS

    void Start() {
        resistorOhms = 4200f;
    }

  public float getResistorOhms()
    {
        return resistorOhms;
    }
    
}
