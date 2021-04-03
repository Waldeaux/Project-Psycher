using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNetworkRigidbody : NetworkBehaviour
{

    [SyncVar(hook = "NetworkKinematic")]
    public bool networkKinematic;

    [SyncVar(hook = "NetworkUseGravity")]
    public bool networkUseGravity;

    [SyncVar(hook = "LogNetworkRb")]
    public NetworkRigidbodyStruct networkRb;

    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        networkKinematic = rb.isKinematic;
        RefreshStruct();
    }

    public void RefreshStruct(bool useTransform = false)
    {
        if (useTransform)
        {
            rb.position = transform.position;
            rb.rotation = transform.rotation;
        }
        networkRb = new NetworkRigidbodyStruct(
            rb.position,
            rb.rotation,
            rb.velocity,
            rb.angularVelocity);
    }
    private void FixedUpdate()
    {
        if (networkKinematic)
        {
            return;
        }
        if (isServer)
        {
            if ((networkRb.position - rb.position).magnitude > .01f ||
                Quaternion.Angle(networkRb.rotation, rb.rotation) > .1f)
            {
                networkRb = new NetworkRigidbodyStruct(
                    rb.position,
                    rb.rotation,
                    rb.velocity,
                    rb.angularVelocity);
            }
        }
        else
        {
            float scale = .9f;
            rb.position = networkRb.position;
            rb.rotation = networkRb.rotation;
            rb.velocity = networkRb.velocity;
            rb.angularVelocity = networkRb.angularVelocity;
        }
    }

    private void LogNetworkRb(NetworkRigidbodyStruct oldValue, NetworkRigidbodyStruct newValue)
    {
        //print(oldValue);
    }
    private void NetworkKinematic(bool oldValue, bool newValue)
    {
        if (rb)
        {
            rb.isKinematic = newValue;
        }
    }
    private void NetworkUseGravity(bool oldValue, bool newValue)
    {
        if (rb)
        {
            rb.useGravity = newValue;
        }
    }

    [Command]
    private void CmdUpdateNetworkRigidbody(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {
        networkRb = new NetworkRigidbodyStruct(
            position,
            rotation,
            velocity,
            angularVelocity);
    }

    public struct NetworkRigidbodyStruct
    {
        public NetworkRigidbodyStruct(
            Vector3 position,
            Quaternion rotation,
            Vector3 velocity,
            Vector3 angularVelocity
            )
        {
            this.position = position;
            this.rotation = rotation;
            this.velocity = velocity;
            this.angularVelocity = angularVelocity;
        }
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
        public Vector3 angularVelocity;
    }
}
