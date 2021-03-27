using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindGameObjectsInLayer : MonoBehaviour
{
    GameObject[] allObjects = FindObjectsOfType<GameObject>();
    List<GameObject> objectsInLayer = new List<GameObject>();
    public List<GameObject> Name(string layerName)
    {
        foreach(GameObject gameObject in allObjects)
        {
            if (LayerMask.LayerToName(gameObject.layer) == layerName)
            {
                objectsInLayer.Add(gameObject);
            }
        }
        return objectsInLayer;
    }
}
