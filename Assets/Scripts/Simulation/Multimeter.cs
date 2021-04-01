using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Multimeter : MonoBehaviour
{
    private float redVolt, redAmp, blackVolt, blackAmp, displayedVoltage, displayedAmp;
     
    // Start is called before the first frame update
    void Start()
    {
        displayValues();
    } 

    // Update is called once per frame
    void Update()
    {
        displayValues();
    }

    public void calcAmpBetweenTwoPoints(){
        
    }

    public void calcVoltBetweenTwoPoints(){
        
    }

    public void displayValues(){
        
        GameObject.FindWithTag("TextTMP").GetComponent<TextMeshProUGUI>().text = "Amp: " + displayedAmp + "\n Voltage: " + displayedVoltage;
    }
}
