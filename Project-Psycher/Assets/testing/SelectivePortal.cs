using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectivePortal : Portal
{
    public GameObject allowedObject;
    public override void HandleTriggerEnter(Collider other)
    {
        print(other.gameObject == allowedObject);
        if (other.gameObject == allowedObject)
        {
            base.HandleTriggerEnter(other);
        }
    }
}
