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
    public GameObject workspace;
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
        calculatedCentre = GetCentre;
        calculatedDirection = (transform.position - GetCentre);
        SetIntendedDistance = (transform.position - GetCentre).magnitude;
        workspace = GameObject.Find("Workspace");
        currentSimulationMode = GameObject.Find("Simulation Event Handler").GetComponent<SimulationMethods>().currentSimulationMode;
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
        calculatedPosition = GetCentre + calculatedDirection.normalized * intendedDistance;

        //Actions
        transform.position = Vector3.Slerp(transform.position, calculatedPosition, Time.deltaTime/smoothness);
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
        offset = bound.center - centre.position;
    }

    public void ViewModeZoomToComponent(Transform HitTarget)
    {
        Centre = HitTarget;
        GetObjectInSight();
        GameObject.Find("Camera Event Handler").GetComponent<OrbitCameraEventPublisher>().ViewModeZoomedCamera();
    }
    public void ZoomToWorkspace()
    {
        Centre = workspace.transform;
        GetObjectInSight();
    }

}
