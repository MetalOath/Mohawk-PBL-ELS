using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Multimeter : MonoBehaviour
{
    [SerializeField] private float voltageReading, currentReading;
    [SerializeField] private GameObject ampsPort, milliAmpsPort, commonPort, voltsOhmsPort, screenTMP;
    private MultimeterPort ampsPortScript, milliAmpsPortScript, commonPortScript, voltsOhmsPortScript;

    // Start is called before the first frame update
    void Start()
    {
        ampsPortScript = ampsPort.GetComponent<MultimeterPort>();
        milliAmpsPortScript = milliAmpsPort.GetComponent<MultimeterPort>();
        commonPortScript = commonPort.GetComponent<MultimeterPort>();
        voltsOhmsPortScript = voltsOhmsPort.GetComponent<MultimeterPort>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayReadings();
    }

    private void CalculateCurrentBetweenTwoPoints()
    {
        currentReading = commonPortScript.currentReading + ampsPortScript.currentReading + milliAmpsPortScript.currentReading;
    }

    private void CalculateVoltageBetweenTwoPoints()
    {
        voltageReading = commonPortScript.voltageReading + voltsOhmsPortScript.voltageReading;
    }

    private void DisplayReadings()
    {
        CalculateCurrentBetweenTwoPoints();
        CalculateVoltageBetweenTwoPoints();
        screenTMP.GetComponent<TextMeshProUGUI>().text = "Current: " + currentReading + " Amps" + 
            "\n Voltage: " + voltageReading + " Volts";
    }
}