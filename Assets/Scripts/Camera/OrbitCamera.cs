using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OrbitCamera : MonoBehaviour
{
    [SerializeField, Tooltip("The orbit centre")]
    private Transform centre;


    [SerializeField, Tooltip("The speed of the scroll centre")]
    private float ScrollSpeed = 10;

    [SerializeField, Tooltip("The angle value per pixel moved")]
    private float OrbitSpeed = 5;

    [SerializeField, Tooltip("The smoothness of the orbit rotation"), Range(0.01f, 1)]
    private float smoothness = 0.5f;

    [SerializeField, Tooltip("The minimum distance possible from the object")]
    private float minDistance = 1;

    [SerializeField, Tooltip("The maximum distance possible from the object")]
    private float maxDistance = 20;


    #region accessors
    public Transform Centre { get { return centre; } set { centre = value; } }
    public GameObject SetCentre { set { centre = value.transform; } }
    public Vector3 GetCentre { get { return centre.position + offset; }}
    public float SetMinDistance { set { minDistance = value; } }
    public float SetMaxDistance { set { maxDistance = value; } }
    public float SetIntendedDistance { set { intendedDistance = Mathf.Clamp(value, minDistance, maxDistance); } }

    public string currentSimulationMode;
    public bool zoomedToElement = false, breadboardCamera = false;
    public GameObject workspace;

    public SimulationMethods Simulation;
    public WireInstantiator WireInstantiator;
    public OrbitCameraEventPublisher CameraEventPublisher;
    public UIEventMethods UIEventMethods;
    public ElementInstantiator ElementInstantiator;
    #endregion


    #region calculations
    private float intendedDistance;
    private Vector3 offset;
    private Vector3 calculatedCentre;
    private Vector3 calculatedDirection;
    private Vector3 calculatedPosition;
    #endregion

    public void Start()
    {
        // Sets minimum camera proximity to objects to allow greater zoom when running on mobile.
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            minDistance = 0.25f;

        // Providing access to methods in other scripts within the scene.
        Simulation = GameObject.Find("Simulation Event Handler").GetComponent<SimulationMethods>();
        WireInstantiator = GameObject.Find("Simulation Event Handler").GetComponent<WireInstantiator>();
        CameraEventPublisher = GameObject.Find("Camera Event Handler").GetComponent<OrbitCameraEventPublisher>();
        UIEventMethods = GameObject.Find("UI Event Handler").GetComponent<UIEventMethods>();
        ElementInstantiator = GameObject.Find("Simulation Event Handler").GetComponent<ElementInstantiator>();

        calculatedCentre = GetCentre;
        calculatedDirection = (transform.position - GetCentre);
        SetIntendedDistance = (transform.position - GetCentre).magnitude;

        workspace = GameObject.Find("Workspace");
        Simulation.ActivateViewMode();
        WireInstantiator.SetWireColor(WireInstantiator.redWireMat);
        currentSimulationMode = Simulation.currentSimulationMode;
        ZoomToWorkspace();
    }


    public abstract void UserInput();

    
    public void Update()
    {
        UserInput();
    }

    // This will do all the calculations
    private void LateUpdate()
    {
        // Calculations
        calculatedCentre = Vector3.Lerp(calculatedCentre, GetCentre, Time.deltaTime / smoothness);
        if (breadboardCamera)
        calculatedPosition = GetCentre + Vector3.up * intendedDistance;
        else
        calculatedPosition = GetCentre + calculatedDirection.normalized * intendedDistance;

        //Actions
        transform.position = Vector3.Slerp(transform.position, calculatedPosition, Time.deltaTime/smoothness);
        if (breadboardCamera)
            transform.forward = Vector3.Lerp(transform.forward, -Centre.forward, Time.deltaTime / smoothness);
        else
            transform.forward = Vector3.Lerp(transform.forward, calculatedCentre - transform.position, Time.deltaTime / smoothness);
        
    }
    
    /// <summary>
    /// Calculates the rotation change compared to the centre of orbit
    /// </summary>
    /// <param name="axisInputX"></param>
    /// <param name="axisInputY"></param>
    /// <returns></returns>
    protected Quaternion PerformRotate(float axisInputX, float axisInputY)
    {
        Quaternion angle = Quaternion.AngleAxis(axisInputX * OrbitSpeed, transform.up);
        angle *= Quaternion.AngleAxis(axisInputY * OrbitSpeed, -transform.right);

        calculatedDirection = angle * calculatedDirection;

        return angle;
    }

    /// <summary>
    /// Calculates the required distance for the camera to match the user zoom input 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    protected float PerformZoom(float input)
    {
        float delta = input * ScrollSpeed;
        SetIntendedDistance = intendedDistance + delta;

        return intendedDistance;
    }

    /// <summary>
    /// Create an offset from the centre object to move side ways
    /// </summary>
    /// <param name="axisInputX"></param>
    /// <param name="axisInputY"></param>
    /// <returns></returns>
    protected Vector3 PerformPan(float axisInputX, float axisInputY)
    {
        Vector3 panValue = transform.right * axisInputX + transform.up * axisInputY ;
        offset += panValue;

        return panValue;
    }

    public void ResetOffset()
    {
        offset = new Vector3();
    }

    /// <summary>
    /// Gets the boundaries of an object using a renderer
    /// </summary>
    /// <returns></returns>
    private Bounds GetBounds()
    {
        // Retrieve all Renderers
        Renderer[] renderers = centre.GetComponentsInChildren<Renderer>();

        // Create a bound located at the position of the first found renderer
        Bounds bound = new Bounds(renderers[0].bounds.center, renderers[0].bounds.size);
        
        // Extend the previous bounds to include all found renderers 
        foreach (Renderer rdr in renderers)
        {
            bound.Encapsulate(rdr.bounds);
        }

        // return the generated bound
        return bound;
    }
    
    /// <summary>
    /// Gets the camera to zoom out in order to have the entirety of the object in sight
    /// </summary>
    public void GetObjectInSight()
    {
        // 
        Bounds bound = GetBounds();

        // Get a camera
        Camera camera = GetComponent<Camera>()??Camera.main;

        // We work with a circle around the object
        float radius = bound.extents.magnitude;
        
        // Get the horizontal FOV, since it may be the limiting of the two FOVs to properly encapsulate the objects
        float horizontalFOV = 2f * Mathf.Atan(Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2f) * camera.aspect) * Mathf.Rad2Deg;

        // Determine which FOV is the most limiting to have the object in sight
        float fov = Mathf.Min(camera.fieldOfView, horizontalFOV);

        // Set the new distance
        SetIntendedDistance = radius / (Mathf.Sin(fov * Mathf.Deg2Rad / 2f));

        // Ensures the real object centre is looked at
        if (breadboardCamera)
            offset = Vector3.zero;
        else
            offset = bound.center - centre.position;
    }

    // Zooms to the raycast target in different ways depending on the current mode and the target's contents.
    public void ZoomToElement([SerializeField] Transform HitTarget)
    {
        if (currentSimulationMode == "ViewMode")
        {
            CameraEventPublisher.ViewModeZoomedCamera();
            Centre = HitTarget;
        }
        else if (currentSimulationMode == "EditMode")
        {
            CameraEventPublisher.EditModeZoomedCamera();
            ShowSelectionPoints();
            if (HitTarget.Find("EditZone"))
                Centre = HitTarget.Find("EditZone");
            else
                Centre = HitTarget;
        }
        else if (currentSimulationMode == "ConnectMode")
        {
            CameraEventPublisher.ConnectModeZoomedCamera();
            ShowConnectionPoints();
            if (HitTarget.parent.CompareTag("Breadboard"))
                ActivateBreadboardCamera(HitTarget);
            else if (HitTarget.Find("ConnectZone"))
                Centre = HitTarget.Find("ConnectZone");
            else
                Centre = HitTarget;
        }

        GetObjectInSight();
        zoomedToElement = true;
    }

    // Zooms out the camera to fit all elements in the scene.
    public void ZoomToWorkspace()
    {
        Centre = workspace.transform;
        GetObjectInSight();
        zoomedToElement = false;
        HideConnectionPoints();
        HideSelectionPoints();
    }

    // Zooms out to show the area that elements are allowed to spawn.
    public void ZoomToElementSpawnZone(Transform transform)
    {
        Centre = transform;
        GetObjectInSight();
    }
    public void ShowConnectionPoints()
    {
        UIEventMethods.UpdateGameObjectList();
        if (UIEventMethods.connectionPoints.Count > 0)
        foreach (GameObject CP in UIEventMethods.connectionPoints)
        {
            StartCoroutine(WaitBeforeActivation(CP, 0.5f));
        }
    }
    public void HideConnectionPoints()
    {
        UIEventMethods.UpdateGameObjectList();
        if (UIEventMethods.connectionPoints.Count > 0)
        foreach (GameObject CP in UIEventMethods.connectionPoints)
        {
            CP.SetActive(false);
        }
    }
    public void ShowSelectionPoints()
    {
        UIEventMethods.UpdateGameObjectList();
        if (UIEventMethods.selectionPoints.Count > 0)
        foreach (GameObject SP in UIEventMethods.selectionPoints)
        {
            StartCoroutine(WaitBeforeActivation(SP, 0.5f));
        }
    }
    public void HideSelectionPoints()
    {
        UIEventMethods.UpdateGameObjectList();
        if (UIEventMethods.selectionPoints.Count > 0)
        foreach (GameObject SP in UIEventMethods.selectionPoints)
        {
            SP.SetActive(false);
        }
    }
    public void ShowSpawnColliders()
    {
        UIEventMethods.UpdateGameObjectList();
        if (UIEventMethods.spawnColliders.Count > 0)
            foreach (GameObject SC in UIEventMethods.spawnColliders)
            {
                SC.SetActive(true);
            }
    }
    public void HideSpawnColliders()
    {
        UIEventMethods.UpdateGameObjectList();
        if (UIEventMethods.spawnColliders.Count > 0)
            foreach (GameObject SC in UIEventMethods.spawnColliders)
            {
                SC.SetActive(false);
            }
    }
    public void ActivateBreadboardCamera(Transform breadboard)
    {
        breadboardCamera = true;
        Centre = breadboard;
        GetObjectInSight();
        zoomedToElement = true;
        ShowConnectionPoints();
    }
    public void DisableBreadboardCamera()
    {
        HideConnectionPoints();
        breadboardCamera = false;
        zoomedToElement = false;
        ZoomToWorkspace();
    }
    IEnumerator WaitBeforeActivation(GameObject go, float waitTime)
    {
        //Do something before waiting.


        //yield on a new YieldInstruction that waits for X seconds.
        yield return new WaitForSeconds(waitTime);

        //Do something after waiting.
        if (zoomedToElement)
        go.SetActive(true);
    }
}