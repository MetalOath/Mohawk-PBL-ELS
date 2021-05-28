using UnityEngine;

public class OrbitCameraEventMethods : OrbitCamera
{
    public bool receives1FingerInput, receives2FingerInput, receives3FingerInput;
    public bool allowMiddleMouse, allowLeftMouse, allowRightMouse, allowScroll;

    private float touchTime;

    public override void UserInput()
    {
        currentSimulationMode = Simulation.currentSimulationMode;
        
        // MOBILE INPUT
        // Checks if platform is Mobile
        if (Simulation.platform == "mobile")// && !Simulation.IsPointerOverGameObject())
        {
            // Since script is run on every frame, returns null in case there is no input.
            if (Input.touchCount == 0)
            {
                return;
            }

            // When touch input is detected, do the following.
            foreach (Touch touch in Input.touches)
            {
                // Records time of touch detection.
                if (touch.phase == TouchPhase.Began)
                {
                    touchTime = Time.time;
                    return;
                }

                // If currently in Element Spawn Mode, do the following.
                if (Simulation.inElementSpawnPhase)
                {
                    // If the user lifted their finger from the screen and the touch time was more than 1 second long, place the element in its last postion.
                    if (touch.phase == TouchPhase.Ended && (Time.time - touchTime) > 1f)
                    {
                        Simulation.ElementInstantiator.PlaceElement();
                    }
                }
                // Otherwise, do the following.
                else
                {
                    // If in View Mode, and input touch time is less than 0.2 seconds, do the following.
                    if (currentSimulationMode == "ViewMode" && touch.phase == TouchPhase.Ended && (Time.time - touchTime) < 0.2f)
                    {
                        // If not already zoomed to an element, zoom on selected element.
                        if (!zoomedToElement)
                            ZoomToElement();
                    }

                    // If in Edit Mode, and input touch time is less than 0.2 seconds, do the following.
                    if (currentSimulationMode == "EditMode" && touch.phase == TouchPhase.Ended && (Time.time - touchTime) < 0.2f)
                    {
                        // If not already zoomed to an element, zoom on selected element.
                        if (!zoomedToElement)
                            ZoomToElement();
                        // If already zoomed to an element, execute selected element's selection point function (if any).
                        if (zoomedToElement)
                            InvokeElementEvent();
                    }

                    // If in Connect Mode, and input touch time is less than 0.2 seconds, do the following.
                    if (currentSimulationMode == "ConnectMode" && touch.phase == TouchPhase.Ended && (Time.time - touchTime) < 0.2f)
                    {
                        // If not already zoomed to an element, zoom on selected element.
                        if (!zoomedToElement)
                            ZoomToElement();
                        // If already zoomed to an element, start wire spawn phase.
                        if (zoomedToElement)
                            Simulation.WireInstantiator.WireSpawnPhaseInitiator();
                    }

                    // If input touch time is less than 0.2 seconds, do the following.
                    if (touch.phase == TouchPhase.Ended && (Time.time - touchTime) < 0.2f)
                    {
                        // If in Delete Phase, start element delete routine.
                        if (Simulation.inDeletePhase)
                            GetElementToDelete();
                    }
                }
            }

            // If not in Element spawn phase do the following.
            if (!Simulation.inElementSpawnPhase)
                // Depending on input touch count, 1, 2, or 3, do the following.
            switch (Input.touchCount)
            {
                // If only one finger is used, do the following.
                case 1:
                    // Stops the script from running after removing finger(s) from screen.
                    if (!receives1FingerInput)
                        break;
                    // If breadboard camera is active, pan instead of orbit.
                    if (breadboardCamera)
                        PerformPan(Input.GetTouch(0).deltaPosition.x * -0.001f, Input.GetTouch(0).deltaPosition.y * -0.001f);
                    // Otherwise, orbit around the object.
                    else
                        PerformRotate(Input.GetTouch(0).deltaPosition.x * 0.02f, Input.GetTouch(0).deltaPosition.y * 0.02f);
                    break;
                case 2:
                    // Stops the script from running after removing finger(s) from screen.
                    if (!receives2FingerInput)
                        break;
                    // Perform zoom based on the 2 fingers' displacement.
                    PerformZoom(FingerToFingerDelta() * 0.002f);
                    break;
                case 3:
                    // Stops the script from running after removing finger(s) from screen.
                    if (!receives3FingerInput)
                        break;
                    // Orbit around the object on the horizontal axis only.
                    PerformRotate(Input.GetTouch(0).deltaPosition.x * 0.01f, 0);
                    break;

            }
        }

        // DESKTOP INPUT
        // Checks if platform is Desktop, and that the mouse pointer is not over a UI element.
        if (Simulation.platform == "desktop" && !Simulation.IsPointerOverGameObject())
        {
            // When left mouse button is pressed, record the time.
            if (Input.GetMouseButtonDown(0))
            {
                touchTime = Time.time;
            }
            // If currently in Element Spawn Mode, do the following.
            if (Simulation.inElementSpawnPhase)
            {
                // When left mouse button is pressed, place the element at the pointer's location.
                if (Input.GetMouseButtonDown(0))
                {
                    Simulation.ElementInstantiator.PlaceElement();
                }
            }
            // Otherwise, do the following.
            else
            {
                // If leftmouse button is allowed, and when left mouse button is pressed, do the following.
                if (allowLeftMouse && Input.GetMouseButton(0))
                {
                    // If breadboard camera is active, pan instead of orbit.
                    if (breadboardCamera)
                        PerformPan(Input.GetAxis("Mouse X") * -0.02f, Input.GetAxis("Mouse Y") * -0.02f);
                    // Otherwise, orbit around the object.
                    else
                        PerformRotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                }
                // If rightmouse button is allowed, and when right mouse button is pressed, perform camera pan.
                if (allowRightMouse && Input.GetMouseButton(1))
                {
                    PerformPan(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                }
                // If middlemouse button is allowed, and when middle mouse button is pressed, perform camera orbit on horizontal axis.
                if (allowMiddleMouse && Input.GetMouseButton(2))
                {
                    PerformRotate(Input.GetAxis("Mouse X"), 0);
                }
                // If scroll wheel is allowed, and when scroll wheel is spun, perform camera zoom.
                if (allowScroll && Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    PerformZoom(Input.GetAxis("Mouse ScrollWheel"));
                }
                // If leftmouse button is released, and currently in View Mode and mouse click time is less than 0.2 seconds, do the following.
                if (Input.GetMouseButtonUp(0) && currentSimulationMode == "ViewMode" && (Time.time - touchTime) < 0.2f)
                {
                    // If currently not zoomed to element, zoom to clicked element.
                    if (!zoomedToElement)
                        ZoomToElement();
                }
                // If leftmouse button is released, and currently in Edit Mode and mouse click time is less than 0.2 seconds, do the following.
                if (Input.GetMouseButtonUp(0) && currentSimulationMode == "EditMode" && (Time.time - touchTime) < 0.2f)
                {
                    // If not zoomed to element, zoom to clicked element.
                    if (!zoomedToElement)
                        ZoomToElement();
                    // If already zoomed to an element, execute selected element's selection point function (if any).
                    if (zoomedToElement)
                        InvokeElementEvent();
                }
                // If leftmouse button is released, and currently in Connect Mode and mouse click time is less than 0.2 seconds, do the following.
                if (Input.GetMouseButtonUp(0) && currentSimulationMode == "ConnectMode" && (Time.time - touchTime) < 0.2f)
                {
                    // If not zoomed to element, zoom to clicked element.
                    if (!zoomedToElement)
                        ZoomToElement();
                    // If already zoomed to an element, start wire spawn phase.
                    if (zoomedToElement)
                        Simulation.WireInstantiator.WireSpawnPhaseInitiator();
                }
                // If leftmouse button is released, and mouse click time is less than 0.2 seconds, do the following.
                if (Input.GetMouseButtonUp(0) && (Time.time - touchTime) < 0.2f)
                {
                    // If in Delete Phase, start element delete routine.
                    if (Simulation.inDeletePhase)
                        GetElementToDelete();
                }
            }
        }
    }
    // Gets distance between 2 fingers for zooming in mobile.
    private float FingerToFingerDelta()
    {
        Vector3 previousPosA = Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition;
        Vector3 previousPosB = Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition;

        float previousDelta = Vector3.Distance(previousPosA, previousPosB);
        float currentDelta = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

        return currentDelta - previousDelta;
    }
    // multiple touch input calculations. (Unused)
    private bool GroupedFingers()
    {
        return Vector2.SqrMagnitude(Input.GetTouch(0).deltaPosition) > 10f &&
            Vector2.SqrMagnitude(Input.GetTouch(1).deltaPosition) > 10 &&
            Vector2.Angle(Input.GetTouch(0).deltaPosition, Input.GetTouch(1).deltaPosition) < 90;
    }
    // zooms the camera to clicked/touched element in the scene.
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
    // 
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

    private void GetElementToDelete()
    {
        Ray raycast = Simulation.SingleRayCastByPlatform();
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            Simulation.ElementInstantiator.DeleteElementInitializer(raycastHit.collider.transform.gameObject);
        }
    }
}