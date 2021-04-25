using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Multimeter : MonoBehaviour
{
    [SerializeField] private float voltageReading, currentReading, resistanceReading;
    [SerializeField] private GameObject ampsPort, milliAmpsPort, commonPort, voltsOhmsPort, dial, screenTMP;
    private MultimeterPort ampsPortScript, milliAmpsPortScript, commonPortScript, voltsOhmsPortScript;
    private string multimeterMode = "OFF1";

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

    private void GetReadings()
    {
        if (commonPortScript.isConnected && voltsOhmsPortScript.isConnected)
        {
            switch (multimeterMode)
            {
                case "AC Voltage":
                    if (commonPortScript.voltageReading >= voltsOhmsPortScript.voltageReading)
                        voltageReading = commonPortScript.voltageReading;
                    else
                        voltageReading = voltsOhmsPortScript.voltageReading;
                    break;
                case "DC Voltage":
                    if (commonPortScript.voltageReading >= voltsOhmsPortScript.voltageReading)
                        voltageReading = commonPortScript.voltageReading;
                    else
                        voltageReading = voltsOhmsPortScript.voltageReading;
                    break;
                case "Resistance/Continuiy/Diode/Capacitance":
                    if (commonPortScript.resistanceReading >= voltsOhmsPortScript.resistanceReading)
                        resistanceReading = commonPortScript.resistanceReading;
                    else
                        resistanceReading = voltsOhmsPortScript.resistanceReading;
                    break;
            }
        }
        else if (commonPortScript.isConnected && milliAmpsPortScript.isConnected)
        {
            switch (multimeterMode)
            {
                case "MicroAmps":
                    currentReading = commonPortScript.currentReading;
                    break;
                case "MilliAmps":
                    currentReading = commonPortScript.currentReading;
                    break; 
            }
        }
        else if (commonPortScript.isConnected && ampsPortScript.isConnected)
        {
            currentReading = commonPortScript.currentReading;
        }
        else
        {
            voltageReading = 0f;
            currentReading = 0f;
            resistanceReading = 0f;
        }
    }

    private void DisplayReadings()
    {
        GetReadings();

        switch (multimeterMode)
        {
            case "AC Voltage":
                screenTMP.GetComponent<TextMeshProUGUI>().text = "Voltage: " + voltageReading + " Volts";
                break;
            case "DC Voltage":
                screenTMP.GetComponent<TextMeshProUGUI>().text = "Voltage: " + voltageReading + " Volts";
                break;
            case "Resistance/Continuiy/Diode/Capacitance":
                screenTMP.GetComponent<TextMeshProUGUI>().text = "Resistance: " + resistanceReading + " Ohms";
                break;
            case "MicroAmps":
                screenTMP.GetComponent<TextMeshProUGUI>().text = "Current: " + currentReading + " Amps";
                break;
            case "MilliAmps":
                screenTMP.GetComponent<TextMeshProUGUI>().text = "Current: " + currentReading + " Amps";
                break;
            case "Amps":
                screenTMP.GetComponent<TextMeshProUGUI>().text = "Current: " + currentReading + " Amps";
                break;
            case "OFF1":
            case "OFF2":
                screenTMP.GetComponent<TextMeshProUGUI>().text = "";
                break;
        }
    }

    public void SetMultimeterMode([SerializeField] string mode)
    {
        switch (mode)
        {
            case "OFF1":
                dial.transform.localRotation = Quaternion.Euler(-90f, 0f, -21f);
                multimeterMode = "OFF1";
                break;
            case "AC Voltage":
                dial.transform.localRotation = Quaternion.Euler(-90f, 0f, -1.7f);
                multimeterMode = "AC Voltage";
                break;
            case "DC Voltage":
                dial.transform.localRotation = Quaternion.Euler(-90f, 0f, 15f);
                multimeterMode = "DC Voltage";
                break;
            case "Resistance/Continuiy/Diode/Capacitance":
                dial.transform.localRotation = Quaternion.Euler(-90f, 0f, 35f);
                multimeterMode = "Resistance/Continuiy/Diode/Capacitance";
                break;
            case "MicroAmps":
                dial.transform.localRotation = Quaternion.Euler(-90f, 0f, 103f);
                multimeterMode = "MicroAmps";
                break;
            case "MilliAmps":
                dial.transform.localRotation = Quaternion.Euler(-90f, 0f, 121f);
                multimeterMode = "MilliAmps";
                break;
            case "Amps":
                dial.transform.localRotation = Quaternion.Euler(-90f, 0f, 138f);
                multimeterMode = "Amps";
                break;
            case "OFF2":
                dial.transform.localRotation = Quaternion.Euler(-90f, 0f, 159f);
                multimeterMode = "OFF2";
                break;
        }
    }
}