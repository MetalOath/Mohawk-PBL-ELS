using UnityEngine;

public class OrbitCameraEventMethods : OrbitCamera
{
    public bool receives1FingerInput, receives2FingerInput, receives3FingerInput;
    public bool allowMiddleMouse, allowLeftMouse, allowRightMouse, allowScroll;

    private float touchTime;

    public override void UserInput()
    {
        currentSimulationMode = GameObject.Find("Simulation Event Handler").GetComponent<SimulationMethods>().currentSimulationMode;

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount == 0)
            {
                return;
            }

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    touchTime = Time.time;
                    return;
                }

                if (currentSimulationMode == "ViewMode" && touch.phase == TouchPhase.Ended && (Time.time - touchTime) < 0.2f)
                {
                    ZoomToElement();
                }

                if (currentSimulationMode == "EditMode" && touch.phase == TouchPhase.Ended && (Time.time - touchTime) < 0.2f)
                {
                    ZoomToElement();
                    InvokeElementEvent();
                }

                if (currentSimulationMode == "ConnectMode" && touch.phase == TouchPhase.Ended && (Time.time - touchTime) < 0.2f)
                {
                    WireInstantiator.WireSpawnPhaseInitiator();
                }
            }

            switch (Input.touchCount)
            {
                case 1:
                    if (!receives1FingerInput)
                        break;

                    if (breadboardCamera)
                    {
                        PerformPan(Input.GetTouch(0).deltaPosition.x * 0.01f, Input.GetTouch(0).deltaPosition.y * 0.02f);
                    }
                    else
                    {
                        PerformRotate(Input.GetTouch(0).deltaPosition.x * 0.02f, Input.GetTouch(0).deltaPosition.y * 0.02f);
                    }
                    break;
                case 2:
                    if (!receives2FingerInput)
                        break;

                    PerformZoom(FingerToFingerDelta() * 0.002f);
                    break;
                case 3:
                    if (!receives3FingerInput)
                        break;

                    PerformRotate(Input.GetTouch(0).deltaPosition.x * 0.01f, 0);
                    break;

            }
        }
        
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchTime = Time.time;
            }

            if (allowLeftMouse && Input.GetMouseButton(0))
            {
                if (breadboardCamera)
                {
                    PerformPan(Input.GetAxis("Mouse X") * -0.02f, Input.GetAxis("Mouse Y") * -0.02f);
                }
                else
                {
                    PerformRotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                }
            }

            if (allowRightMouse && Input.GetMouseButton(1))
            {
                PerformPan(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }

            if (allowMiddleMouse && Input.GetMouseButton(2))
            {
                PerformRotate(Input.GetAxis("Mouse X"), 0);
            }

            if (allowScroll && Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                PerformZoom(Input.GetAxis("Mouse ScrollWheel"));
            }

            if (Input.GetMouseButtonUp(0) && currentSimulationMode == "ViewMode" && (Time.time - touchTime) < 0.2f)
            {
                ZoomToElement();
            }

            if (Input.GetMouseButtonUp(0) && currentSimulationMode == "EditMode" && (Time.time - touchTime) < 0.2f)
            {
                ZoomToElement();
                if(zoomedToElement == true)
                InvokeElementEvent();
            }

            if (Input.GetMouseButtonUp(0) && currentSimulationMode == "ConnectMode" && (Time.time - touchTime) < 0.2f)
            {
                ZoomToElement();
                if(zoomedToElement == true)
                WireInstantiator.WireSpawnPhaseInitiator();
            }
        }
    }

    private float FingerToFingerDelta()
    {
        Vector3 previousPosA = Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition;
        Vector3 previousPosB = Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition;

        float previousDelta = Vector3.Distance( previousPosA, previousPosB);
        float currentDelta = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

        return currentDelta - previousDelta;

    }

    private bool GroupedFingers()
    {
        return Vector2.SqrMagnitude(Input.GetTouch(0).deltaPosition) > 10f &&
            Vector2.SqrMagnitude(Input.GetTouch(1).deltaPosition) > 10 &&
            Vector2.Angle(Input.GetTouch(0).deltaPosition, Input.GetTouch(1).deltaPosition) < 90;
    }

    private void ZoomToElement()
    {
        Ray raycast = Simulation.SingleRayCastByPlatform();
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            if (raycastHit.collider.CompareTag("Interactive"))
            {
                ZoomToElement(raycastHit.collider.transform);
            }
        }
    }

    private void InvokeElementEvent()
    {
        Ray raycast = Simulation.SingleRayCastByPlatform();

        RaycastHit[] raycastHits = Physics.RaycastAll(raycast, 100f);
        RaycastHit raycastHit;
        for (int i = 0; i < raycastHits.Length; i++)
        {
            if (raycastHits[i].transform.gameObject.CompareTag("Selection_Points"))
            {
                raycastHit = raycastHits[i];
                ElementEventPublisher raycastHitSelectionPoint = raycastHit.transform.gameObject.GetComponent<ElementEventPublisher>();
                if (raycastHitSelectionPoint)
                {
                    raycastHitSelectionPoint.InvokeElementMethods();
                    break;
                }
            }
        }
    }
}