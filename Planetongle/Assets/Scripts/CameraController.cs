using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Planet RotatingAround;

    public Camera Camera;

    public float PlanetRotateSpeed;
    public float SprintSpeedMultiplier;

    public float CameraZoomDefault;
    public float CameraZoomMin;
    public float CameraZoomMax;

    public float CameraZoomFractionToGoToFullView;

    public float ViewTransitionTime;

    public float FullViewCameraSize = 100f;

    bool _isLerpingPlanetView;
    bool _isAtFullPlanetView;

    float _cameraZoomFraction;

    void Awake()
    {
        transform.parent = RotatingAround.GroundViewCameraParent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Camera.orthographicSize = CameraZoomDefault;
    }

    void Update()
    {
        updateZooming();

        if (_cameraZoomFraction < CameraZoomFractionToGoToFullView)
        {
            if (_isAtFullPlanetView)
            {
                RotatingAround.CameraRotator.localEulerAngles = Vector3.zero;

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

        float startZAngle = transform.localEulerAngles.z;
        Vector3 startLocalPosition = transform.localPosition;

        float startCameraSize = Camera.orthographicSize;

        float timer = 0f;
        while (timer < ViewTransitionTime)
        {
            Vector3 eulerAngles = transform.localEulerAngles;
            eulerAngles.z = Mathf.LerpAngle(startZAngle, 0f, timer / ViewTransitionTime);
            transform.localEulerAngles = eulerAngles;

            transform.localPosition = Vector3.Lerp(startLocalPosition, Vector3.zero, timer / ViewTransitionTime);

            Camera.orthographicSize = Mathf.Lerp(startCameraSize, FullViewCameraSize, timer / ViewTransitionTime);

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
            angleDelta = PlanetRotateSpeed * Time.deltaTime;
        }
        else if (moveRight)
        {
            angleDelta = PlanetRotateSpeed * Time.deltaTime * -1f;
        }

        if (sprint)
            angleDelta *= SprintSpeedMultiplier;

        RotatingAround.RotateCameraArondCenter(angleDelta);
    }

    void updateFullPlanetView()
    {
    }

    void updateZooming()
    {
        float scrollDelta = Input.mouseScrollDelta.y;

        float size = Camera.orthographicSize + (scrollDelta * -1);
        size = Mathf.Clamp(size, CameraZoomMin, CameraZoomMax);

        Camera.orthographicSize = size;

        _cameraZoomFraction = (size - CameraZoomMin) / CameraZoomMax;
    }
}
