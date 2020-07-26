using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public Planet OrbitingAround;

    public Transform GroundViewCameraParent;
    public Transform CameraRotator;
    public Transform FullViewCameraParent;

    public float Mass;

    public float AtmosphereRadius;

    public float CameraRotateSpeed;
    public float CameraSprintSpeedMultiplier;

    public float CameraZoomDefault;
    public float CameraZoomMin;
    public float CameraZoomMax;

    public float CameraZoomFractionToGoToFullView;

    public float CameraViewTransitionTime;

    public float FullViewCameraSize;

    public bool IsInitialized { get; private set; }

    void Start()
    {
        PlanetManager.Instance.OnPlanetCreated(this);
    }

    void OnEnable()
    {
        if (PlanetManager.Instance != null)
            PlanetManager.Instance.OnPlanetCreated(this);
    }

    void OnDisable()
    {
        if (PlanetManager.Instance != null)
            PlanetManager.Instance.OnPlanetDestroyed(this);
    }

    void OnDestroy()
    {
        if (PlanetManager.Instance != null)
            PlanetManager.Instance.OnPlanetDestroyed(this);
    }

    public void Initialize()
    {
        if (IsInitialized)
            return;

        IsInitialized = true;
    }

    public void RotateCameraArondCenter(float angleDelta)
    {
        Vector3 localEulerAngles = CameraRotator.localEulerAngles;
        localEulerAngles.z += angleDelta;
        CameraRotator.localEulerAngles = localEulerAngles;
    }

    public void SetCameraAngleZ(float zAngle)
    {
        Vector3 localEulerAngles = CameraRotator.localEulerAngles;
        localEulerAngles.z = zAngle;
        CameraRotator.localEulerAngles = localEulerAngles;
    }

    public float GetCameraZAngleDeltaTo(Vector3 globalPosition)
    {
        return Utils.GetZAngleDeltaFromDirection(Utils.Get2DDirection(CameraRotator.localPosition, CameraRotator.InverseTransformPoint(globalPosition)));
    }

    void FixedUpdate()
    {
        if (OrbitingAround != null)
        {
            transform.RotateAround(OrbitingAround.transform.position, Vector3.forward, 10f * Time.deltaTime);
        }
    }
}
