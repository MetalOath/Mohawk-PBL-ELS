﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Simulation : MonoBehaviour
{
    public bool inWireSpawnPhase = false;
    public Transform connectionPointOne, connectionPointTwo;

    public GameObject wirePrefab;
    public GameObject wireSegmentPrefab;
    private GameObject wirePrefabSegmentMeasurementInstance;
    private Transform wireContainerTransform;
    private float wirePrefabLength;
    private float wireSegmentLength;
    private float wireLength;
    private float distanceBetweenPoints;
    private float yFunction;
    private int numberOfSegments;

    public bool simulationActiveState = false;
    public string currentSimulationMode;

    public void Start()
    {
        wirePrefabSegmentMeasurementInstance = Instantiate(wireSegmentPrefab, Vector3.zero, Quaternion.identity);
        wirePrefabLength = wirePrefabSegmentMeasurementInstance.GetComponent<Collider>().bounds.size.y;
        Destroy(wirePrefabSegmentMeasurementInstance);

        wireContainerTransform = GameObject.Find("Wires").transform;

        wireSegmentLength = Mathf.Abs(wirePrefabLength * 0.8f);
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

        GameObject wireInstance = Instantiate(wirePrefab, pointOne.position + (pointTwo.position - pointOne.position)/2, Quaternion.identity);
        wireInstance.name = "Wire " + (int)Time.time;
        wireInstance.transform.parent = wireContainerTransform;

        GameObject previousSegment = null;
        Quaternion pointTwoInvertedRotation = new Quaternion(pointTwo.rotation.x, pointTwo.rotation.y, pointTwo.rotation.z + 180f, pointTwo.rotation.w);

        for (int i = 0; i <= numberOfSegments; i++)
        {
            yFunction = Mathf.Sqrt(Mathf.Pow(distanceBetweenPoints / 2f, 2f) - Mathf.Pow((i * distanceBetweenPoints/numberOfSegments - distanceBetweenPoints / 2f), 2f));
            Vector3 parabolicY = new Vector3(0f, yFunction, 0f);
            Vector3 pathToPointTwo = (pointTwo.position - pointOne.position) / numberOfSegments * i;
            Vector3 midPoint = pointOne.position + (pointTwo.position - pointOne.position) / 2f;

            Vector3 createPosition = pointOne.position + parabolicY + pathToPointTwo;
            Quaternion createRotation = Quaternion.FromToRotation(pointTwo.position - pointOne.position, midPoint - createPosition);

            GameObject currentSegment = Instantiate(wireSegmentPrefab, createPosition, createRotation);

            currentSegment.transform.parent = wireInstance.transform;
            
            if (i == 0)
            {
                currentSegment.GetComponent<Rigidbody>().isKinematic = true;
                previousSegment = currentSegment;
                currentSegment.transform.position = pointOne.position;
                currentSegment.transform.rotation = pointOne.rotation;

                CapsuleCollider cc = wireInstance.AddComponent<CapsuleCollider>();
                cc.isTrigger = true;
                cc.center = currentSegment.transform.localPosition;
                cc.radius = currentSegment.GetComponent<Collider>().bounds.size.x;
                cc.height = currentSegment.GetComponent<Collider>().bounds.size.y;
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

                CapsuleCollider cc = wireInstance.AddComponent<CapsuleCollider>();
                cc.isTrigger = true;
                cc.center = currentSegment.transform.localPosition;
                cc.radius = currentSegment.GetComponent<Collider>().bounds.size.x;
                cc.height = currentSegment.GetComponent<Collider>().bounds.size.y;
            }
        }
    }
}
