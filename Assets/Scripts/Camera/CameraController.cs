using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject mainCamera, editModeButton, zoomOutButton, workspace;
    private string currentGameMode;
    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        editModeButton = GameObject.Find("Button - Edit Mode");
        workspace = GameObject.Find("Workspace");
        zoomOutButton = workspace.GetComponent<CameraController>().zoomOutButton;
        currentGameMode = GameObject.Find("Simulation Event Handler").GetComponent<SimulationMethods>().currentGameMode;
    }
    void Update()
    {
        if (currentGameMode == "ViewMode")
        {
            ComponentZoomMobile();
        }
        
    }
    private void ComponentZoomMobile()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                //Debug.Log("Something Hit");
                //if (raycastHit.collider.name == "Example")
                //{
                //    Debug.Log("Example Clicked");
                //}

                //OR with Tag

                if (raycastHit.collider.CompareTag("Interactive"))
                {
                    mainCamera.GetComponent<OrbitCameraMobile>().Centre = raycastHit.collider.transform;
                    mainCamera.GetComponent<OrbitCameraMobile>().GetObjectInSight();
                    editModeButton.SetActive(false);
                    zoomOutButton.SetActive(true);
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("Interactive"))
                {
                    mainCamera.GetComponent<OrbitCameraDesktop>().Centre = raycastHit.collider.transform;
                    mainCamera.GetComponent<OrbitCameraDesktop>().GetObjectInSight();
                    editModeButton.SetActive(false);
                    zoomOutButton.SetActive(true);
                }
            }
        }
    }
    public void ZoomToWorkspace()
    {
        mainCamera.GetComponent<OrbitCameraMobile>().Centre = workspace.transform;
        mainCamera.GetComponent<OrbitCameraMobile>().GetObjectInSight();
        mainCamera.GetComponent<OrbitCameraDesktop>().Centre = workspace.transform;
        mainCamera.GetComponent<OrbitCameraDesktop>().GetObjectInSight();
        editModeButton.SetActive(true);
        zoomOutButton.SetActive(false);
    }
}
