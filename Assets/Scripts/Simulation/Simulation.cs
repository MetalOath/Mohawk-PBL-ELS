using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Simulation : MonoBehaviour
{
    public bool inWireSpawnPhase = false;
    public Transform connectionPointOne, connectionPointTwo;

    public GameObject wirePrefab;
    private GameObject wirePrefabMeasurementInstance;
    private GameObject wireContainer;
    private float wirePrefabLength;
    private float wireSegmentLength;
    private float wireLength;
    private int numberOfSegments;

    public bool simulationActiveState = false;
    public string currentSimulationMode;

    public List<GameObject> nonConnectorObjects = new List<GameObject>();
    public GameObject[] allGameObjects;

    public void Start()
    {
        wirePrefabMeasurementInstance = Instantiate(wirePrefab, Vector3.zero, Quaternion.identity);
        wirePrefabLength = wirePrefabMeasurementInstance.GetComponent<Collider>().bounds.size.y;
        Destroy(wirePrefabMeasurementInstance);

        wireContainer = new GameObject("Wire Container");

        wireSegmentLength = wirePrefabLength - (wirePrefabLength / 5f);
    }
    public void WireSpawnPhaseInitiator()
    {
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] raycastHits = Physics.RaycastAll(raycast, 100f);
        RaycastHit raycastHit;
        for (int i = 0; i< raycastHits.Length; i++)
        {
            if (raycastHits[i].transform.gameObject.CompareTag("Connection_Points"))
            {
                raycastHit = raycastHits[i];
                if (inWireSpawnPhase == false)
                {
                    connectionPointOne = raycastHit.transform;
                    inWireSpawnPhase = true;
                }
                else if (inWireSpawnPhase == true && raycastHit.transform != connectionPointOne)
                {
                    connectionPointTwo = raycastHit.transform;
                    SpawnWire(connectionPointOne, connectionPointTwo);
                    inWireSpawnPhase = false;
                }
                break;
            }
        }
        //if (Physics.Raycast(raycast, out raycastHit))
        //{
        //    if (raycastHit.collider.CompareTag("Connection_Points"))
        //    {
        //        if (inWireSpawnPhase == false)
        //        {
        //            connectionPointOne = raycastHit.transform;
        //            inWireSpawnPhase = true;
        //        }
        //        else if (inWireSpawnPhase == true && raycastHit.transform != connectionPointOne)
        //        {
        //            connectionPointTwo = raycastHit.transform;
        //            SpawnWire(connectionPointOne, connectionPointTwo);
        //            inWireSpawnPhase = false;
        //        }
        //    }
        //}
    }

    private void SpawnWire(Transform pointOne, Transform pointTwo)
    {
        wireLength = Vector3.Magnitude(pointTwo.position - pointOne.position); // use pi*d/2
        numberOfSegments = (int)(wireLength/wireSegmentLength) + 1;

        float angleDifferenceZ = Vector3.Angle(pointOne.forward, pointTwo.forward);
        float angleDifferenceY = Vector3.Angle(pointOne.up, pointTwo.up);

        GameObject wireContainerInstance = Instantiate(wireContainer, pointOne.position + (pointTwo.position - pointOne.position)/2, Quaternion.identity);

        for (int i = 0; i <= numberOfSegments; i++)
        {
            Vector3 createPos = pointOne.position + (pointTwo.position - pointOne.position) / numberOfSegments * i;
            //Quaternion createRot = Quaternion.LerpUnclamped(pointOne.rotation, Quaternion.Inverse(pointTwo.rotation), (float)i/numberOfSegments);
            GameObject currentSegment = Instantiate(wirePrefab, createPos, Quaternion.identity);
            float rotX = pointOne.rotation.x * (1 - (float)i / numberOfSegments) + (angleDifferenceY + 90) * ((float)i / numberOfSegments),
                rotY = pointOne.rotation.y * (1 - (float)i / numberOfSegments) + (angleDifferenceZ) * ((float)i / numberOfSegments),
                rotZ = pointOne.rotation.z * (1 - (float)i / numberOfSegments) + (angleDifferenceY + 90) * ((float)i / numberOfSegments);
            currentSegment.transform.Rotate(-pointTwo.up * ((float)i / numberOfSegments));

            currentSegment.transform.parent = wireContainerInstance.transform;
        }
    }

    //public void DeactivateNonConnectorColliders()
    //{
    //    allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
    //    foreach (GameObject nonConnector in allGameObjects)
    //    {
    //        if (!nonConnector.CompareTag("Connection_Points") && !nonConnector.CompareTag("Wire") && nonConnector.GetComponent<Collider>())
    //        {
    //            nonConnectorObjects.Add(nonConnector);
    //            nonConnector.GetComponent<Collider>().enabled = false;
    //        }
    //    }
    //}

    //public void ActivateNonConnectorColliders()
    //{
    //    foreach (GameObject nonConnector in nonConnectorObjects)
    //    {
    //        nonConnector.GetComponent<Collider>().enabled = true;
    //    }
    //}
}
