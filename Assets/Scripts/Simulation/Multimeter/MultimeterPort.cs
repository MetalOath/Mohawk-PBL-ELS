using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultimeterPort : MonoBehaviour
{
    public float voltageReading, currentReading;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerStay(Collider other)
    {
        Conduction otherObjectConduction = other.GetComponentInParent<Conduction>();
        if (otherObjectConduction)
        {
            voltageReading = otherObjectConduction.voltage;
            currentReading = otherObjectConduction.current;
        }
    }
}