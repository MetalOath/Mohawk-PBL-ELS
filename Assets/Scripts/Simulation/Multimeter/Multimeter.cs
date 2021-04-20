using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Multimeter : MonoBehaviour
{
    [SerializeField] private float voltageReading, currentReading;
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

    private void GetCurrentReading()
    {
        currentReading = commonPortScript.currentReading;// + ampsPortScript.currentReading + milliAmpsPortScript.currentReading;
    }

    private void GetVoltageReading()
    {
        voltageReading = commonPortScript.voltageReading;// + voltsOhmsPortScript.voltageReading;
    }

    private void DisplayReadings()
    {
        GetCurrentReading();
        GetVoltageReading();
        if (multimeterMode == "OFF1" || multimeterMode == "OFF2")
        {
            screenTMP.GetComponent<TextMeshProUGUI>().text = "";
        }
        if (multimeterMode == "Amps" || multimeterMode == "MilliAmps" || multimeterMode == "MicroAmps")
        {
            screenTMP.GetComponent<TextMeshProUGUI>().text = "Current: " + currentReading + " Amps";
        }
        if (multimeterMode == "AC Voltage" || multimeterMode == "DC Voltage")
        {
            screenTMP.GetComponent<TextMeshProUGUI>().text = "Voltage: " + voltageReading + " Volts";
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