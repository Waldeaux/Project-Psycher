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

    public void SwitchToOffset()
    {
        camera.SwitchToOffset();
    }

    public void SwitchToSpectate()
    {
        camera.SwitchToSpectate();
    }

    public void SwitchSpectate(GameObject target)
    {
        camera.SwitchSpectate(target);
    }
}
