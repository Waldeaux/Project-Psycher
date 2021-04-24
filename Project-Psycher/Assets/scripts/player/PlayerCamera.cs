using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public CameraFollow camera;

    public void SwitchToFollow()
    {
        camera.SwitchToFollow();
    }

    public void SwitchToImmobile()
    {
        camera.SwitchToImmobile();
    }
}
