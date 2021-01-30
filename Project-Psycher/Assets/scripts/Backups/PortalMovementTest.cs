using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMovementTest : MonoBehaviour
{
    PortalHole portal;
    private void Start()
    {
        portal = GetComponent<PortalHole>();
    }
    void Update()
    {
        Vector3 offset = (Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right) * Time.deltaTime * 5;
        transform.position += offset;
        if (portal)
        {
            portal.OffsetPortal(offset);
        }
    }
}
