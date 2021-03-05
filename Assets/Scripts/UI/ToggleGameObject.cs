using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    public void Toggle()
    {
        if (gameObject.activeInHierarchy == true)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
