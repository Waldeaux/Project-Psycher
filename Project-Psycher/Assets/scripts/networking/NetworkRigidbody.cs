using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRigidbody : NetworkBehaviour
{
    [SyncVar(hook = "NetworkKinematic")]
    public bool networkKinematic;

    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void NetworkKinematic(bool oldValue, bool newValue)
    {
        if (rb)
        {
            rb.isKinematic = newValue;
        }
    }
}
