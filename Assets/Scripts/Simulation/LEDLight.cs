using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDLight : MonoBehaviour
{
    private bool n, p;
    public GameObject ledLight;
    private void Update()
    {
        n = gameObject.GetComponent<Conduction>().negativePassThrough;
        p = gameObject.GetComponent<Conduction>().positivePassThrough;
        if (n && p){
            ledLight.SetActive(true);
        }
        else
        {
            ledLight.SetActive(false);
        }
    }
}
