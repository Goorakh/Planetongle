using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Planet RotatingAround;

    public Camera Camera;

    bool _isLerpingPlanetView;
    bool _isAtFullPlanetView;

    float _cameraZoomFraction;

    void Awake()
    {
        transform.parent = RotatingAround.GroundViewCameraParent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Camera.orthographicSize = RotatingAround.CameraZoomDefault;
    }

    void Update()
    {
        if (!_isLerpingPlanetView)
            updateZooming();

        if (_cameraZoomFraction < RotatingAround.CameraZoomFractionToGoToFullView)
        {
            if (_isAtFullPlanetView)
            {
                Camera.transform.SetParent(RotatingAround.GroundViewCameraParent);
                Camera.transform.localPosition = Vector3.zero;
                Camera.transform.localEulerAngles = Vector3.zero;

                _isAtFullPlanetView = false;
            }

            updateSidewaysMovement();
        }
        else
        {
            if (!_isAtFullPlanetView)
            {
                if (!_isLerpingPlanetView)
                    StartCoroutine(lerpToFullView());
                
                return;
            }

            updateFullPlanetView();
        }
    }

    IEnumerator lerpToFullView()
    {
        _isLerpingPlanetView = true;

        Camera.transform.SetParent(RotatingAround.FullViewCameraParent, true);

        Vector3 startLocalPosition = transform.localPosition;

        float startCameraSize = Camera.orthographicSize;

        float timer = 0f;
        while (timer < RotatingAround.CameraViewTransitionTime)
        {
            transform.localPosition = Vector3.Lerp(startLocalPosition, Vector3.zero, timer / RotatingAround.CameraViewTransitionTime);

            Camera.orthographicSize = Mathf.Lerp(startCameraSize, RotatingAround.FullViewCameraSize, timer / RotatingAround.CameraViewTransitionTime);

            timer += Time.deltaTime;
            yield return 0;
        }

        _isLerpingPlanetView = false;
        _isAtFullPlanetView = true;
    }

    void updateSidewaysMovement()
    {
        bool moveLeft = InputManager.Instance.IsInputActive(ActionType.PlanetCameraMoveLeft);
        bool moveRight = InputManager.Instance.IsInputActive(ActionType.PlanetCameraMoveRight);

        if (moveLeft == moveRight)
            return;

        bool sprint = InputManager.Instance.IsInputActive(ActionType.PlanetCameraMoveSprint);

        float angleDelta = 0f;
        if (moveLeft)
        {
            angleDelta = RotatingAround.CameraRotateSpeed * Time.deltaTime;
        }
        else if (moveRight)
        {
            angleDelta = RotatingAround.CameraRotateSpeed * Time.deltaTime * -1f;
        }

        if (sprint)
            angleDelta *= RotatingAround.CameraSprintSpeedMultiplier;

        RotatingAround.RotateCameraArondCenter(angleDelta);
    }

    void updateFullPlanetView()
    {
        if (InputManager.Instance.IsInputActive(ActionType.PlanetFullViewSelectPosition))
        {
            float zAngleDelta = RotatingAround.GetCameraZAngleDeltaTo(Camera.ScreenToWorldPoint(Input.mousePosition));

            Camera.transform.SetParent(RotatingAround.GroundViewCameraParent, true);

            StartCoroutine(lerpToGroundView(zAngleDelta));
        }
    }
    IEnumerator lerpToGroundView(float zAngleDelta)
    {
        _isLerpingPlanetView = true;

        Vector3 startCameraLocalPosition = Camera.transform.localPosition;
        Vector3 startCameraLocalEulerAngles = Camera.transform.localEulerAngles;

        float startZ = RotatingAround.CameraRotator.localEulerAngles.z;
        float targetAngle = startZ + zAngleDelta;

        float startCameraSize = Camera.orthographicSize;

        float timer = 0f;
        while (timer < RotatingAround.CameraViewTransitionTime)
        {
            float fraction = timer / RotatingAround.CameraViewTransitionTime;

            Camera.transform.localPosition = Vector3.Lerp(startCameraLocalPosition, Vector3.zero, fraction);
            Camera.transform.localEulerAngles = Utils.LerpEulerAngles(startCameraLocalEulerAngles, Vector3.zero, fraction);

            RotatingAround.SetCameraAngleZ(Mathf.LerpAngle(startZ, targetAngle, fraction));

            Camera.orthographicSize = Mathf.Lerp(startCameraSize, RotatingAround.CameraViewTransitionTime, fraction);
            updateZoomFraction();

            timer += Time.deltaTime;
            yield return 0;
        }

        _isLerpingPlanetView = false;
        _isAtFullPlanetView = false;
    }

    void updateZooming()
    {
        float scrollDelta = Input.mouseScrollDelta.y;

        float size = Camera.orthographicSize - scrollDelta;
        size = Mathf.Clamp(size, RotatingAround.CameraZoomMin, RotatingAround.CameraZoomMax);

        Camera.orthographicSize = size;
        updateZoomFraction();
    }

    void updateZoomFraction()
    {
        _cameraZoomFraction = (Camera.orthographicSize - RotatingAround.CameraZoomMin) / RotatingAround.CameraZoomMax;
    }
}
