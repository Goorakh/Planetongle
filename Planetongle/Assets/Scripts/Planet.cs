using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public Transform GroundViewCameraParent;
    public Transform CameraRotator;
    public Transform FullViewCameraParent;

    public void RotateCameraArondCenter(float angleDelta)
    {
        Vector3 localEulerAngles = CameraRotator.localEulerAngles;
        localEulerAngles.z += angleDelta;
        CameraRotator.localEulerAngles = localEulerAngles;
    }
}
