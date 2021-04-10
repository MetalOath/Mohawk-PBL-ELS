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
    private float distanceBetweenPoints;
    private int numberOfSegments;

    public bool simulationActiveState = false;
    public string currentSimulationMode;

    public void Start()
    {
        wirePrefabMeasurementInstance = Instantiate(wirePrefab, Vector3.zero, Quaternion.identity);
        wirePrefabLength = wirePrefabMeasurementInstance.GetComponent<Collider>().bounds.size.y;
        Destroy(wirePrefabMeasurementInstance);

        wireSegmentLength = wirePrefabLength * 0.8f;
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
    }

    private void SpawnWire(Transform pointOne, Transform pointTwo)
    {
        distanceBetweenPoints = Vector3.Magnitude(pointTwo.position - pointOne.position);
        wireLength = Mathf.PI * distanceBetweenPoints / 2f; // use pi*d/2
        numberOfSegments = (int)(wireLength/wireSegmentLength) + 1;

        wireContainer = new GameObject("Wire Container");
        GameObject wireContainerInstance = Instantiate(wireContainer, pointOne.position + (pointTwo.position - pointOne.position)/2, Quaternion.identity);
        Destroy(wireContainer);

        GameObject previousSegment = null;
        Quaternion pointTwoInvertedRotation = new Quaternion(pointTwo.rotation.x, pointTwo.rotation.y, pointTwo.rotation.z + 180f, pointTwo.rotation.w);
        //Quaternion pointTwoInvertedRotation = Quaternion.Inverse(pointTwo.rotation);

        GameObject wirePathFinder = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        wirePathFinder.transform.position = pointOne.position + new Vector3(0, -wireLength, 0) + (pointTwo.position - pointOne.position)/2f;
        wirePathFinder.transform.localScale = wirePathFinder.transform.localScale * Mathf.Abs(Vector3.Magnitude(pointTwo.position - pointOne.position));

        for (int i = 0; i <= numberOfSegments; i++)
        {
            //Vector3 createPos = new Vector3(pointOne.position.x, pointOne.position.y+(i*wireSegmentLength), pointOne.position.z);

            Vector3 createPosition = pointOne.position + new Vector3(0, (distanceBetweenPoints / 2f) - (distanceBetweenPoints / 2f) * Mathf.Abs((numberOfSegments / 2f - i) / (numberOfSegments / 2f)), 0) + (pointTwo.position - pointOne.position) / numberOfSegments * i;
            //Vector3 createPosition = pointOne.position + new Vector3(0, , 0) + (pointTwo.position - pointOne.position) / numberOfSegments * i;
            Quaternion createRotation = Quaternion.FromToRotation(Vector3.up, pointTwo.position - pointOne.position);
            GameObject currentSegment = Instantiate(wirePrefab, createPosition, createRotation);
            //currentSegment.transform.rotation = Quaternion.FromToRotation(currentSegment.transform.up, (pointOne.position + (pointTwo.position - pointOne.position) / 2f));

            currentSegment.transform.parent = wireContainerInstance.transform;
            
            if (i == 0)
            {
                currentSegment.GetComponent<Rigidbody>().isKinematic = true;
                previousSegment = currentSegment;
                currentSegment.transform.position = pointOne.position;
                currentSegment.transform.rotation = pointOne.rotation;
            }

            if (i != 0)
            {
                currentSegment.GetComponent<ConfigurableJoint>().connectedBody = previousSegment.GetComponent<Rigidbody>();
                previousSegment = currentSegment;
            }

            if (i == numberOfSegments)
            {
                currentSegment.GetComponent<Rigidbody>().isKinematic = true;
                currentSegment.transform.position = pointTwo.position;
                currentSegment.transform.rotation = pointTwoInvertedRotation;
            }
        }
        
        //spherePathFinder.transform.position = pointOne.position + new Vector3(0, -wireLength*0.15f, 0) + (pointTwo.position - pointOne.position) / 2f;
        //Destroy(spherePathFinder);
    }
}
