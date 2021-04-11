using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Multimeter : MonoBehaviour
{
    [SerializeField] private float redVolt, redAmp, blackVolt, blackAmp, displayedVoltage, displayedAmp;
     
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("REACHED MULTIMETER SCRIPT");
        displayValues();
    } 

    // Update is called once per frame
    void Update()
    {
        displayValues();
    }

    public void calcAmpBetweenTwoPoints(){
        displayedAmp = redAmp + blackAmp;  
    }

    public void calcVoltBetweenTwoPoints(){
        displayedVoltage = redVolt + blackVolt; 
        
    }
        
    // TO DO LATER: 
        // if the circuit is open, either the black or red wire values should be 0.
     private void OnTriggerStay(Collider other)
    {
        //Debug.Log("REACHED TRIGGER ON STAY");
        // instance of GameObject that is being collided with 
        GameObject otherObject = other.gameObject;
        Conduction otherObjectConduction = otherObject.GetComponent<Conduction>();

        if (otherObjectConduction)
        {
            if(otherObject.tag == "multiBlackCable") {
                blackVolt = otherObjectConduction.voltage;
                blackAmp = otherObjectConduction.current;
            }
            if(otherObject.tag =="multiRedCable"){
                redVolt = otherObjectConduction.voltage;
                redAmp = otherObjectConduction.current;
            }
        }
    }
    public void displayValues(){
        // Debug.Log("BLACKVOLT DISPLAY: " + blackVolt + " BLACKAMP DISPLAY :  " + blackAmp);
        // Debug.Log("redVolt DISPLAY: " + redVolt + " REDAMP DISPLAY: " + redAmp);
        calcAmpBetweenTwoPoints();
        calcVoltBetweenTwoPoints();
        GameObject.FindWithTag("TextTMP").GetComponent<TextMeshProUGUI>().text = "Amp: " + displayedAmp + "\n Voltage: " + displayedVoltage;

    }
}
