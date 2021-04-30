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

        int numberOfInstances = CheckElementsInWorkspace(elementName);

        switch (elementName)
        {
            case ("Breadboard"):
                if (numberOfInstances < 2)
                {
                    newElement = element;
                    
                    SpawnElement();
                }
                else
                {
                    CancelSpawn();
                    Simulation.DisplayErrorMessage("ONLY 2 BREADBOARDS ALLOWED");
                }
                break;
            case ("Multimeter"):
                if (numberOfInstances < 1)
                {
                    newElement = element;

                    SpawnElement();
                }
                else
                {
                    CancelSpawn();
                    Simulation.DisplayErrorMessage("ONLY 1 MULTIMETER ALLOWED");
                }
                break;
            case ("Battery - 9V"):
                if (numberOfInstances < 3)
                {
                    newElement = element;

                    SpawnElement();
                }
                else
                {
                    CancelSpawn();
                    Simulation.DisplayErrorMessage("ONLY 3 9V BATTERIES ALLOWED");
                }
                break;
            case ("Battery - 1.5V"):
                if (numberOfInstances < 3)
                {
                    newElement = element;

                    SpawnElement();
                }
                else
                {
                    CancelSpawn();
                    Simulation.DisplayErrorMessage("ONLY 3 1.5V BATTERIES ALLOWED");
                }
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
        UIEventMethods.ClearUI();
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

    public void CancelSpawn()
    {
        if(newElementInstance)
        Destroy(newElementInstance);
        Simulation.inElementSpawnPhase = false;
        if (elementSpawnZone.activeInHierarchy)
            elementSpawnZone.SetActive(false);
        newElementInstance = null;
        UIEventMethods.UpdateGameObjectList();
        UIEventPublisher.EditModeUI();
    }

    private int CheckElementsInWorkspace(string elementName)
    {
        List<GameObject> listOfElements = new List<GameObject>();
        int numberOfInstances = 0;

        for (int i = 0; i < workspace.childCount; i++)
        {
            listOfElements.Add(workspace.GetChild(i).gameObject);
        }

        foreach (GameObject workspaceElement in listOfElements)
        {
            if (workspaceElement.name == elementName+"(Clone)")
            {
                numberOfInstances++;
            }
        }

        return numberOfInstances;
    }
}
