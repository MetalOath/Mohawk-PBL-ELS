using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraFocus : MonoBehaviour
{
    private GameObject mainCamera;
    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
    }
    void Update()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                //Debug.Log("Something Hit");
                //if (raycastHit.collider.name == "Soccer")
                //{
                //    Debug.Log("Soccer Ball clicked");
                //}

                //OR with Tag

                //if (raycastHit.collider.CompareTag("SoccerTag"))
                //{
                //    Debug.Log("Soccer Ball clicked");
                //}

                mainCamera.GetComponent<OrbitCameraMobile>().Centre = gameObject.transform;
            }
        }
    }
}
