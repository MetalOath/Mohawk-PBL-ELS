using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElementInstantiator : MonoBehaviour
{
    OrbitCameraEventMethods CameraEventMethods;
    WireInstantiator WireInstantiator;
    SimulationMethods Simulation;
    UIEventPublisher UIEventPublisher;
    UIEventMethods UIEventMethods;

    [SerializeField] public GameObject elementSpawnZone, elementSpawnCanvas, deleteElementConfirmCanvas, deleteElementConfirmMessage;
    
    private GameObject newElement, newElementInstance = null, deleteTarget;
    [SerializeField] private Transform workspace, wireContainer;

    // Start is called before the first frame update
    void Start()
    {
        WireInstantiator = GameObject.Find("Simulation Event Handler").GetComponent<WireInstantiator>();
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
            ElementSpawnPositionUpdater();
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
                    
                    ElementSpawnPhaseInitiator();
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

                    ElementSpawnPhaseInitiator();
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

                    ElementSpawnPhaseInitiator();
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

                    ElementSpawnPhaseInitiator();
                }
                else
                {
                    CancelSpawn();
                    Simulation.DisplayErrorMessage("ONLY 3 1.5V BATTERIES ALLOWED");
                }
                break;
            case ("Resistor - 330 Ohm"):
                WireInstantiator.LeadSpawnPhaseInitiator(element, 330f);
                UIEventPublisher.ConnectModeUI();
                break;
            case ("Resistor - 470 Ohm"):
                WireInstantiator.LeadSpawnPhaseInitiator(element, 470f);
                UIEventPublisher.ConnectModeUI();
                break;
            case ("Resistor - 560 Ohm"):
                WireInstantiator.LeadSpawnPhaseInitiator(element, 560f);
                UIEventPublisher.ConnectModeUI();
                break;
            case ("LED Light - Red"):
                WireInstantiator.LeadSpawnPhaseInitiator(element, 0f);
                UIEventPublisher.ConnectModeUI();
                break;
            case ("LED Light - Green"):
                WireInstantiator.LeadSpawnPhaseInitiator(element, 0f);
                UIEventPublisher.ConnectModeUI();
                break;
            case ("LED Light - Blue"):
                WireInstantiator.LeadSpawnPhaseInitiator(element, 0f);
                UIEventPublisher.ConnectModeUI();
                break;
        }
    }

    private void ElementSpawnPhaseInitiator()
    {
        UIEventMethods.ClearUI();
        if (newElementInstance == null)
        {
            if (!elementSpawnZone.activeInHierarchy)
                elementSpawnZone.SetActive(true);
            if (!elementSpawnCanvas.activeInHierarchy)
                elementSpawnCanvas.SetActive(true);

            Simulation.inElementSpawnPhase = true;
            Simulation.currentSimulationMode = "";

            CameraEventMethods.ZoomToElementSpawnZone(elementSpawnZone.transform);
            CameraEventMethods.ShowSpawnColliders();

            newElementInstance = Instantiate(newElement, workspace.position, Quaternion.identity);
            newElementInstance.transform.parent = workspace.transform;
        }
    }

    private void ElementSpawnPositionUpdater()
    {
        Ray raycast = Simulation.SingleRayCastByPlatform();

        RaycastHit[] raycastHits = Physics.RaycastAll(raycast, 100f, -8);
        //RaycastHit raycastHit;

        //for (int i = 0; i < raycastHits.Length; i++)
        //{
            if (raycastHits.Length > 0 && raycastHits[0].transform.gameObject.CompareTag("Element Spawn Point"))
            {
                //raycastHit = raycastHits[0];

                newElementInstance.transform.position = raycastHits[0].point;
            }
            //break;
        //}
    }

    public void PlaceElement()
    {
        if (newElementInstance.transform.Find("SpawnCollider").GetComponent<SpawnCollider>().isIntersecting)
            Simulation.DisplayErrorMessage("CANNOT PLACE OBJECTS ON TOP OF EACHOTHER");
        else
            EndSpawnPhase();
    }

    public void CancelSpawn()
    {
        if (newElementInstance)
            Destroy(newElementInstance);

        EndSpawnPhase();
    }

    public void EndSpawnPhase()
    {
        Simulation.inElementSpawnPhase = false;
        if (elementSpawnZone.activeInHierarchy)
            elementSpawnZone.SetActive(false);
        newElementInstance = null;
        CameraEventMethods.HideSpawnColliders();
        StartCoroutine(PostSpawnTimer(0.25f));
    }

    public void DeleteElementInitializer(GameObject workspaceElement)
    {
        if (workspaceElement.transform.IsChildOf(wireContainer))
        {
            if (workspaceElement.transform.parent.gameObject == wireContainer.gameObject)
            {
                deleteTarget = workspaceElement;
                deleteElementConfirmMessage.GetComponent<TextMeshProUGUI>().text = "Are you sure you want to delete " + deleteTarget.name + "?";
                deleteElementConfirmCanvas.SetActive(true);
            }
            else
            {
                while (workspaceElement.transform.parent.gameObject != wireContainer.gameObject)
                {
                    workspaceElement = workspaceElement.transform.parent.gameObject;
                }

                deleteTarget = workspaceElement;
                deleteElementConfirmMessage.GetComponent<TextMeshProUGUI>().text = "Are you sure you want to delete " + deleteTarget.name + "?";
                deleteElementConfirmCanvas.SetActive(true);
            }
        } else if (workspaceElement.transform.IsChildOf(workspace))
        {
            if (workspaceElement.transform.parent.gameObject == workspace.gameObject)
            {
                deleteTarget = workspaceElement;
                deleteElementConfirmMessage.GetComponent<TextMeshProUGUI>().text = "Are you sure you want to delete " + deleteTarget.name + "?";
                deleteElementConfirmCanvas.SetActive(true);
            }
            else
            {
                while (workspaceElement.transform.parent.gameObject != workspace.gameObject)
                {
                    workspaceElement = workspaceElement.transform.parent.gameObject;
                }

                deleteTarget = workspaceElement;
                deleteElementConfirmMessage.GetComponent<TextMeshProUGUI>().text = "Are you sure you want to delete " + deleteTarget.name + "?";
                deleteElementConfirmCanvas.SetActive(true);
            }
        }
    }

    public void DeleteElement()
    {
        Destroy(deleteTarget);
        Simulation.SetDeletePhase(false);
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

    IEnumerator PostSpawnTimer(float waitTime)
    {
        //Do something before waiting.

        //yield on a new YieldInstruction that waits for X seconds.
        yield return new WaitForSeconds(waitTime);

        //Do something after waiting.
        UIEventPublisher.EditModeUI();
    }
}
