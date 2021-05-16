using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public abstract class Simulation : MonoBehaviour
{
    public UIEventPublisher UIEventPublisher;
    public UIEventMethods UIEventMethods;
    public OrbitCameraEventPublisher CameraEventPublisher;
    public OrbitCameraEventMethods CameraEventMethods;
    public WireInstantiator WireInstantiator;
    public ElementInstantiator ElementInstantiator;

    public List<GameObject> UICanvases = new List<GameObject>();
    public List<GameObject> connectionPoints = new List<GameObject>();
    public List<GameObject> selectionPoints = new List<GameObject>();
    public List<GameObject> spawnColliders = new List<GameObject>();
    GameObject[] allGameObjects;

    public bool simulationActiveState = false;
    public string currentSimulationMode;

    public bool inElementSpawnPhase = false;
    public bool inWireSpawnPhase = false;
    public bool inDeletePhase = false;

    public string platform;

    [SerializeField] private GameObject errorMessageCanvas, errorMessageTMP;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            platform = "mobile";
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            platform = "desktop";
        }

        UIEventPublisher = GameObject.Find("UI Event Handler").GetComponent<UIEventPublisher>();
        UIEventMethods = GameObject.Find("UI Event Handler").GetComponent<UIEventMethods>();
        CameraEventPublisher = GameObject.Find("Camera Event Handler").GetComponent<OrbitCameraEventPublisher>();
        CameraEventMethods = GameObject.Find("Main Camera").GetComponent<OrbitCameraEventMethods>();
        WireInstantiator = GameObject.Find("Simulation Event Handler").GetComponent<WireInstantiator>();
        ElementInstantiator = GameObject.Find("Simulation Event Handler").GetComponent<ElementInstantiator>();
    }
    public void Start()
    {
        if (platform == "mobile")
            CameraEventMethods.minDistance = 0.25f;

        allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        PopulateUICanvasList();
        PopulateCPList();
        PopulateSPList();
    }

    public Ray SingleRayCastByPlatform()
    {
        Ray nullRay = Camera.main.ScreenPointToRay(Vector3.zero);
        // TODO: FIX MOBILE UI RAYCAST BLOCKING
        if (platform == "mobile")
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (!IsPointerOverGameObject())
            {
                return raycast;
            }
            else
            {
                return nullRay;
            }
        }
        else if (platform == "desktop")
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!IsPointerOverGameObject())
            {
                return raycast;
            }
            else
            {
                return nullRay;
            }
        }
        else
        {
            return nullRay;
        }
    }

    public void DisplayErrorMessage(string errorMessage)
    {
        errorMessageTMP.GetComponent<TextMeshProUGUI>().text = errorMessage;

        StartCoroutine(ErrorMessageTimer(3f));
    }

    public void SetDeletePhase(bool isActive)
    {
        inDeletePhase = isActive;
    }

    private void PopulateUICanvasList()
    {
        foreach (GameObject UIElement in allGameObjects)
        {
            if (UIElement.CompareTag("UI_Canvas"))
            {
                UICanvases.Add(UIElement);
            }
        }
    }
    private void PopulateCPList()
    {
        foreach (GameObject CP in allGameObjects)
        {
            if (CP.CompareTag("Connection_Points"))
            {
                connectionPoints.Add(CP);
            }
        }
    }
    private void PopulateSPList()
    {
        foreach (GameObject SP in allGameObjects)
        {
            if (SP.CompareTag("Selection_Points"))
            {
                selectionPoints.Add(SP);
            }
        }
    }
    private void PopulateSCList()
    {
        foreach (GameObject SC in allGameObjects)
        {
            if (SC.CompareTag("SpawnCollider"))
            {
                spawnColliders.Add(SC);
            }
        }
    }
    public void UpdateGameObjectList()
    {
        connectionPoints.Clear();
        selectionPoints.Clear();
        spawnColliders.Clear();
        allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        PopulateCPList();
        PopulateSPList();
        PopulateSCList();
    }

    //testing mobile UI raycast block.
    //private bool IsPointerOverUIObject()
    //{
    //    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    //    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //    List<RaycastResult> results = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    //    return results.Count > 0;
    //}

    /// <returns>true if mouse or first touch is over any event system object ( usually gui elements )</returns>
    public bool IsPointerOverGameObject()
    {
        //check mouse
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        //check touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return true;
        }

        return false;
    }

    IEnumerator ErrorMessageTimer(float waitTime)
    {
        //Do something before waiting.
        errorMessageCanvas.SetActive(true);

        //yield on a new YieldInstruction that waits for X seconds.
        yield return new WaitForSeconds(waitTime);

        //Do something after waiting.
        errorMessageCanvas.SetActive(false);
    }
}