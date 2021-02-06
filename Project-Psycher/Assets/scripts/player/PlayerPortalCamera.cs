using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortalCamera : MainCamera
{
    public void AddPortal(Portal portal)
    {
        portals.Add(portal);
    }

    public void RemovePortal(Portal portal)
    {
        portals.Remove(portal);
    }
}
