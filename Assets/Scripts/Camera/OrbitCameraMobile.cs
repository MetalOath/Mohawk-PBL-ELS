using UnityEngine;

public class OrbitCameraMobile : OrbitCamera
{

#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_STANDALONE || UNITY_EDITOR
    private void OnEnable()
    {
        this.enabled = false;
    }
#endif

    public bool receives1FingerInput, receives2FingerInput, receives3FingerInput;


    public override void UserInput()
    {
        if (Input.touchCount == 0)
        {
            return;
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
                return;
        }

        switch (Input.touchCount)
        {
            case 1:
                if (!receives1FingerInput)
                    break;

                    PerformRotate(Input.GetTouch(0).deltaPosition.x * 0.02f, Input.GetTouch(0).deltaPosition.y * 0.02f);
                break;
            case 2:
                if (!receives2FingerInput)
                    break;

                // If the delta vectors are similar enough then is it a group pan otherwise it is a scale movement
                if (GroupedFingers())
                {
                    PerformPan(Input.GetTouch(0).deltaPosition.x * 0.01f, Input.GetTouch(0).deltaPosition.y * 0.02f);
                }
                else
                {
                    PerformZoom(FingerToFingerDelta() * 0.002f);
                }
                break;
            case 3:
                if (!receives3FingerInput)
                    break;

                PerformRotate(Input.GetTouch(0).deltaPosition.x * 0.01f, 0);
                break;

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
}
