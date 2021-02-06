using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MainCamera
{
    private int layerMask;
    public GameObject followTarget;
    public Vector3 followOffset;

    public void Awake()
    {
        layerMask = 1 << 9;
    }
    public override void PreCullLogic()
    {
        UpdatePosition();
        base.PreCullLogic();
    }
    public void AddPortal(Portal portal)
    {
        portals.Add(portal);
    }
    private void UpdatePosition()
    {
        RaycastHit hitInfo;
        float distance = followOffset.magnitude;
        Vector3 raycastPoint = followTarget.transform.position;
        Vector3 direction = followTarget.transform.TransformDirection(followOffset.normalized);
        Vector3 position = followTarget.transform.position + direction * distance;
        if (Physics.Raycast(raycastPoint, direction, out hitInfo, distance, layerMask, QueryTriggerInteraction.Collide))
        {
            Portal portal = hitInfo.collider.gameObject.GetComponentInParent<Portal>();
            if (portal)
            {
                distance -= hitInfo.distance;
                raycastPoint = hitInfo.point + portal.linkedPortal.transform.position - hitInfo.transform.position;
                direction = TransformDirection(portal, direction);
                Vector3 lookDirection = TransformDirection(portal, followTarget.transform.up);
                position = raycastPoint + direction.normalized * distance;
                transform.position = position;
                transform.rotation = Quaternion.LookRotation(-direction, lookDirection);
                portal.linkedPortal.Render();
            }
        }
        else
        {
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(-direction);
        }
    }

    private Vector3 TransformDirection(Portal portal, Vector3 direction)
    {
        direction = portal.transform.InverseTransformDirection(direction);
        direction = portal.linkedPortal.transform.TransformDirection(direction);
        return direction;
    }
}
