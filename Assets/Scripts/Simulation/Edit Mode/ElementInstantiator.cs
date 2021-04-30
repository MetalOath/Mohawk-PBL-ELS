using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementInstantiator : MonoBehaviour
{
    OrbitCameraEventMethods CameraEventMethods;
    WireInstantiator WireMethods;
    SimulationMethods Simulation;
    UIEventPublisher UIEventPublisher;
    UIEventMethods UIEventMethods;

    [SerializeField] public GameObject elementSpawnZone;
    
    private GameObject newElement, newElementInstance = null;
    [SerializeField] private Transform workspace;

    // Start is called before the first frame update
    void Start()
    {
        WireMethods = GameObject.Find("Simulation Event Handler").GetComponent<WireInstantiator>();
        CameraEventMethods = GameObject.Find("Main Camera").GetComponent<OrbitCameraEventMethods>();
        Simulation = GameObject.Find("Simulation Event Handler").GetComponent<SimulationMethods>();
        UIEventPublisher = GameObject.Find("UI Event Handler").GetComponent<UIEventPublisher>();
        UIEventMethods = GameObject.Find("UI Event Handler").GetComponent<UIEventMethods>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Simulation.inElementSpawnPhase)
        {
            ElementSpawnPosition();
        }
    }

    public void InstantiateElement([SerializeField] GameObject element)
    {
        string elementName = element.name;

        switch (elementName)
        {
            case ("Breadboard"):
                if (workspace.Find("Breadboard(Clone)") == null)
                {
                    newElement = element;
                    
                    SpawnElement();
                }
                break;
            case ("Multimeter"):
                if (workspace.Find("Multimeter(Clone)") == null)
                {
                    newElement = element;

                    SpawnElement();
                }
                break;
            case ("Battery - 9V"):
                if (workspace.Find("Battery - 9V(Clone)") == null)
                {
                    newElement = element;

                    SpawnElement();
                }
                break;
            case ("Battery - 1.5V"):

                break;
            case ("Resistor - 330 Ohm"):

                break;
            case ("Resistor - 470 Ohm"):

                break;
            case ("Resistor - 560 Ohm"):

                break;
            case ("LED Light - Red"):

                break;
            case ("LED Light - Green"):

                break;
            case ("LED Light - Blue"):

                break;
        }
    }

    private void SpawnElement()
    {
        if (newElementInstance == null)
        {
            if (!elementSpawnZone.activeInHierarchy)
                elementSpawnZone.SetActive(true);

            Simulation.inElementSpawnPhase = true;

            CameraEventMethods.ZoomToElementSpawnZone(elementSpawnZone.transform);

            newElementInstance = Instantiate(newElement, workspace.position, Quaternion.identity);
            newElementInstance.transform.parent = workspace.transform;
        }
    }

    private void ElementSpawnPosition()
    {
        Ray raycast = Simulation.SingleRayCastByPlatform();

        RaycastHit[] raycastHits = Physics.RaycastAll(raycast, 100f, -8);
        RaycastHit raycastHit;

        for (int i = 0; i < raycastHits.Length; i++)
        {
            if (raycastHits[i].transform.gameObject.CompareTag("Element Spawn Point"))
            {
                raycastHit = raycastHits[i];

                newElementInstance.transform.position = raycastHit.point;
            }
            break;
        }
    }

    public void PlaceElement()
    {
        //if (newElementInstance.transform.Find("SpawnCollider"))
        Simulation.inElementSpawnPhase = false;
        if (elementSpawnZone.activeInHierarchy)
            elementSpawnZone.SetActive(false);
        newElementInstance = null;
        UIEventMethods.UpdateGameObjectList();
        UIEventPublisher.EditModeUI();
    }

    private void CheckElementsInWorkspace()
    {

    }
}
